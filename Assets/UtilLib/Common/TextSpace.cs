﻿/*
调整文字间距
支持左对齐 右对齐 居中
支持富文本
 */
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
[AddComponentMenu ("UI/Effects/TextSpacing")]
public class TextSpace : BaseMeshEffect {

	public float textSpacing = 1f;
	public float lineSpacing = 1f;

	private Text m_text;

	private void Start () {
		m_text = GetComponent<Text> ();
	}

	private void Update () {

		if (m_text != null && m_text.lineSpacing != lineSpacing) {
			m_text.lineSpacing = lineSpacing;
		}
	}

	struct RichItem {
		public int start; //尖括号索引
		public int end; // 尖括号结束索引
		public bool isFinish;//是否完全匹配
	}

	List<int> richIndexs = new List<int>();
	public override void ModifyMesh (VertexHelper vh) {

		if (!IsActive () || vh.currentVertCount == 0) {
			return;
		}

		if (m_text == null) {
			Debug.LogError ("Missing Text component");
			return;
		}

		List<UIVertex> vertexs = new List<UIVertex> ();
		vh.GetUIVertexStream (vertexs);
		int indexCount = vh.currentIndexCount;

		string[] lineTexts = m_text.text.Split ('\n');

		Line[] lines = new Line[lineTexts.Length];

		if(m_text.supportRichText)
		{
			richIndexs = GetAllRichIndex();
		}

		//根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空母均占6个点
		for (int i = 0; i < lines.Length; i++) {
			//除最后一行外，vertexs对于前面几行都有回车符占了6个点
			if (i == 0) {
				lines[i] = new Line (0, lineTexts[i].Length, vh, vertexs, richIndexs, m_text.supportRichText);
			} else if (i > 0 && i < lines.Length - 1) {
				lines[i] = new Line (lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length, vh, vertexs, richIndexs, m_text.supportRichText);
			} else {
				lines[i] = new Line (lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length, vh, vertexs, richIndexs, m_text.supportRichText, false);
			}
		}

		for (int i = 0; i < lines.Length; i++) {

			if (m_text.alignment == TextAnchor.LowerLeft ||
				m_text.alignment == TextAnchor.MiddleLeft ||
				m_text.alignment == TextAnchor.UpperLeft) {

				lines[i].OffsetByLeft (textSpacing);

			} else if (m_text.alignment == TextAnchor.LowerRight ||
				m_text.alignment == TextAnchor.MiddleRight ||
				m_text.alignment == TextAnchor.UpperRight) {

				lines[i].OffsetByRight (textSpacing);

			} else {

				lines[i].OffsetByCenter (textSpacing);
			}
		}
	}

	//得到所有富文本的索引
	List<int> GetAllRichIndex () {
		if (string.IsNullOrWhiteSpace (m_text.text))
			return null;

		string str = m_text.text;

		Dictionary<int, RichItem> map = new Dictionary<int, RichItem> ();

		Stack<int> stack = new Stack<int> ();

		for (int i = 0; i < str.Length; i++) {
			if (str[i] == '<') {
				RichItem b = new RichItem ();
				b.start = i;
				map.Add (i, b);

				stack.Push (i);
			} else if (str[i] == '>' && stack.Count > 0) {
				int index = stack.Pop ();

				RichItem b;
				if (map.TryGetValue (index, out b)) {
					b.end = i;
					b.isFinish = true;

					map[index] = b;
				}
			}
		}

		List<int> deleteIndexs = new List<int> ();

		var enumerator = map.GetEnumerator ();

		while (enumerator.MoveNext ()) {
			RichItem b = enumerator.Current.Value;
			if (!b.isFinish)
				continue;

			int currentIndex = b.start;

			while (currentIndex <= b.end) {
				if (!deleteIndexs.Contains (currentIndex)) {
					deleteIndexs.Add (currentIndex);
				}
				currentIndex++;
			}
		}

		return deleteIndexs;
	}

	public class singleWord {

		public singleWord front;
		public singleWord next;

		public singleWord (int _start, int _end, List<UIVertex> vertextList, VertexHelper vertexHelper, bool supportrich = false) {
			_vertexHelper = vertexHelper;
			_vertextList = vertextList;
			_startVertexIndex = _start;
			_endVertexIndex = _end;

			supportRich = supportrich;
		}

		//是不是富文本
		public bool isRich = false;		
		public int _startVertexIndex { get; private set; }
		public int _endVertexIndex { get; private set; }

		bool supportRich;
		VertexHelper _vertexHelper;
		List<UIVertex> _vertextList;

		public void excuteOffset (bool dir, int count,float space,float offset) {

			if (dir) { //povit 在右边 向左探索

				if(supportRich)
				{
					if(isRich)
					{
						front?.excuteOffset(dir,count,space,offset);
					}
					else
					{
						front?.excuteOffset(dir,count-1,space,offset);
					}
				}
				else
				{
					front?.excuteOffset(dir,count-1,space,offset);
				}

			} else { //povit 在左边 向右探索

				if(supportRich)
				{
					if(isRich)
					{
						next?.excuteOffset(dir,count,space,offset);
					}
					else
					{
						next?.excuteOffset(dir,count+1,space,offset);
					}
				}
				else
				{
					next?.excuteOffset(dir,count+1,space,offset);
				}
			}

			for (int i = _startVertexIndex; i < _endVertexIndex; i++) {

				if (i >= _vertextList.Count)
					return;

				var vt = _vertextList[i];

				vt.position += new Vector3 (count * space+offset, 0, 0);

				_vertextList[i] = vt;

				//以下注意点与索引的对应关系 每个字占两个面即6个顶点
				if (i % 6 <= 2) {
					_vertexHelper.SetUIVertex (vt, (i / 6) * 4 + i % 6);
				}
				if (i % 6 == 4) {
					_vertexHelper.SetUIVertex (vt, (i / 6) * 4 + i % 6 - 1);
				}
			}
		}
	}

	public class Line {

		/// <summary>
		/// 起点索引
		/// </summary>
		public int StartVertexIndex {get;private set;}

		/// <summary>
		/// 终点索引
		/// </summary>
		public int EndVertexIndex {get;private set;}

		public bool EndEnter { get; private set; }

		List<singleWord> words = new List<singleWord> ();

		public void OffsetByLeft (float textSpacing) {

			int index = 0;

			for (int i = 0; _supportRich && i < words.Count; i++) {
				if (words[i].isRich)
					continue;

				index = i;
				break;
			}

			words[index].excuteOffset(false,0,textSpacing,0);
		}

		public void OffsetByRight (float textSpacing) {

			int index = words.Count - 1;

			for (int i = words.Count - 1; _supportRich && i >= 0; i--) {
				if (words[i].isRich)
					continue;

				index = i;
				break;
			}
			
			words[index].excuteOffset(true,0,textSpacing,0);
		}

		public void OffsetByCenter (float textSpacing) {

			List<singleWord> validWords = new List<singleWord> ();

			if (_supportRich) {
				foreach (var item in words) {
					if (item.isRich)
						continue;
					validWords.Add (item);
				}
			} else {
				validWords = words;
			}

			if (validWords.Count % 2 == 0) { //偶数

				//中间靠右索引
				int centerRIndex = validWords.Count/2;

				//中间靠左索引
				int centerLIndex = validWords.Count/2 -1;

				//这里左右两个字 的偏移权重都是0.5 
				//如果两个字大小相差太大 可能会有点误差 不过这种误差很小 这里忽略不计
				//如要计算权重 可根据 每个字体的大小 分别计算各自的偏移权重 两者偏移权重相加为1
				validWords[centerLIndex].excuteOffset(true,0,textSpacing,-0.5f * textSpacing);
				validWords[centerRIndex].excuteOffset(false,0,textSpacing,0.5f * textSpacing);

			} else { //奇数

				//中间索引
				int centerIndex = (validWords.Count + 1) / 2 - 1;

				validWords[centerIndex].excuteOffset(false,0,textSpacing,0);
				validWords[centerIndex].excuteOffset(true,0,textSpacing,0);
			}
		}

		bool _supportRich = false;

		public Line (int startVertexIndex, int length, VertexHelper vertexHelper, List<UIVertex> vertextList, List<int> richIndexs, bool supportRich, bool enter = true) {

			_supportRich = supportRich;
			int wordLength = length;

			if (enter) {
				length++;
			}

			StartVertexIndex = startVertexIndex;
			EndVertexIndex = length * 6 - 1 + startVertexIndex;

			EndEnter = enter;

			for (int i = 0; i < wordLength; i++) {

				int start = (i + 1) * 6 - 6;
				int end = (i + 1) * 6 - 1;
				singleWord w = new singleWord (start + startVertexIndex, end + startVertexIndex, vertextList, vertexHelper, supportRich);
				words.Add (w);
			}

			for (int i = 0; words.Count > 1 && i < words.Count; i++) {
				if (i == 0) {
					words[i].front = null;
					words[i].next = words[i + 1];
				} else if (i == words.Count - 1) {
					words[i].front = words[i - 1];
					words[i].next = null;
				} else {
					words[i].front = words[i - 1];
					words[i].next = words[i + 1];
				}

				if (supportRich) {
					if (richIndexs != null && richIndexs.Contains (i)) {
						words[i].isRich = true;
					} else {
						words[i].isRich = false;
					}
				}
			}
		}
	}
}