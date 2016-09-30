﻿using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
	[Serializable]
	public class AnimatedTile : BaseTile
	{
		public Sprite[] m_AnimatedSprites = new Sprite[0];
		public float m_MinSpeed = 1f;
		public float m_MaxSpeed = 1f;
		public float m_AnimationTimeOffset;

		public override bool GetTileData(Vector3Int location, ITileMap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0)
			{
				tileData.sprite = m_AnimatedSprites[m_AnimatedSprites.Length - 1];
			}
			return true;
		}

		public override bool GetTileAnimationData(Vector3Int location, ITileMap tileMap, ref TileAnimationData tileAnimationData)
		{
			if (m_AnimatedSprites.Length > 0)
			{
				tileAnimationData.animatedSprites = m_AnimatedSprites;
				tileAnimationData.animationSpeed = Random.Range(m_MinSpeed, m_MaxSpeed);
				tileAnimationData.animationTimeOffset = m_AnimationTimeOffset;
				return true;
			}
			return false;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/Animated Tile")]
		public static void CreateBrush()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Animated Tile", "New Animated Tile", "asset", "Save Animated Tile", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnimatedTile>(), path);
		}

		public override Sprite GetPreview()
		{
			if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0)
			{
				return m_AnimatedSprites[m_AnimatedSprites.Length - 1];

			}
			return null;
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(AnimatedTile))]
	public class AnimatedTileEditor : Editor
	{
		private AnimatedTile tile { get { return (target as AnimatedTile); } }

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			int count = EditorGUILayout.IntField("Number of Animated Sprites", tile.m_AnimatedSprites.Length);
			if (count < 0)
				count = 0;
			if (tile.m_AnimatedSprites == null || tile.m_AnimatedSprites.Length != count)
			{
				Array.Resize<Sprite>(ref tile.m_AnimatedSprites, count);
			}

			if (count == 0)
				return;

			EditorGUILayout.LabelField("Place sprites shown based on the order of animation.");
			EditorGUILayout.Space();

			for (int i = 0; i < count; i++)
			{
				tile.m_AnimatedSprites[i] = (Sprite) EditorGUILayout.ObjectField("Sprite " + (i+1), tile.m_AnimatedSprites[i], typeof(Sprite), false, null);
			}
			
			float minSpeed = EditorGUILayout.FloatField("Minimum Speed", tile.m_MinSpeed);
			float maxSpeed = EditorGUILayout.FloatField("Minimum Speed", tile.m_MaxSpeed);
			if (minSpeed < 0)
				minSpeed = 0.0f;
			if (maxSpeed < 0)
				maxSpeed = 0.0f;
			if (maxSpeed < minSpeed)
			{
				float temp = maxSpeed;
				maxSpeed = minSpeed;
				minSpeed = temp;
			}
			
			tile.m_MinSpeed = minSpeed;
			tile.m_MaxSpeed = maxSpeed;

			tile.m_AnimationTimeOffset = EditorGUILayout.FloatField("Time Offset", tile.m_AnimationTimeOffset);
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);
		}
	}
#endif
}
