using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class NewTestScript
    {
        private MyPlayer player;
        GameObject text1obj = new GameObject("text1");
        GameObject text2obj = new GameObject("text2");
        Text text1;
        Text text2;
        List<Text> names;

        GameObject points1obj = new GameObject("points1");
        GameObject points2obj = new GameObject("points2");
        Text points1;
        Text points2;
        List<Text> points;


        [SetUp]
        public void SetUp()
        {
            text1obj.AddComponent<Text>();
            text2obj.AddComponent<Text>();
            points1obj.AddComponent<Text>();
            points2obj.AddComponent<Text>();

            text1 = text1obj.GetComponent<Text>();
            text2 = text2obj.GetComponent<Text>();
            points1 = points1obj.GetComponent<Text>();
            points2 = points2obj.GetComponent<Text>();
            List<Text> names = new List<Text>();
            text1.text = "name1";
            text1.color = Color.white;
            text2.text = "name2";
            text2.color = Color.white;
            names.Add(text1);
            names.Add(text2);

            List<Text> points = new List<Text>();
            points1.text = "0";
            points2.text = "0";
            points.Add(points1);
            points.Add(points2);

            player = new MyPlayer("player", names, points);

        }


        [Test]
        public void isUp()
        {
            Assert.AreEqual(player.IsUp(), false);
        }

        [Test]
        public void ChangePoints()
        {
            player.AddPoints(-100);
            Assert.AreEqual(player.GetPoints(), -100);
        }

        [Test]
        public void SetTurn()
        {
            player.SetTurn(true);
            Assert.AreEqual(text1.color, Color.green);
            Assert.AreEqual(text2.color, Color.green);
            player.SetTurn(false);
            Assert.AreEqual(text1.color, Color.white);
            Assert.AreEqual(text2.color, Color.white);
        }

        [Test]
        public void BanUnbanTest()
        {
            player.banPlayer();
            Assert.AreEqual(text1.color, Color.red);
            Assert.AreEqual(text2.color, Color.red);

            player.unbanPlayer();
            Assert.AreEqual(text1.color, Color.white);
            Assert.AreEqual(text2.color, Color.white);

        }
    }
}
