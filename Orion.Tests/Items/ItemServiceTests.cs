﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Orion.Items;
using Orion.Items.Events;

namespace Orion.Tests.Items
{
	[TestFixture]
	public class ItemServiceTests
	{
		private static readonly object[] ItemSetDefaultsTestCases = {ItemType.IronPickaxe};

		private static readonly object[] ItemSettingDefaultsTestCases = {ItemType.IronPickaxe};

		private static readonly object[] CreateTestCases =
		{
			new object[] {ItemType.IronPickaxe, 1, ItemPrefix.None},
			new object[] {ItemType.IronPickaxe, 100, ItemPrefix.None},
			new object[] {ItemType.IronPickaxe, 100, ItemPrefix.Legendary}
		};

		private static readonly Predicate<IItem>[] FindTestCases = { item => (int)item.Type < 100 };

		private static readonly object[] SpawnTestCases =
		{
			new object[] {ItemType.IronPickaxe, 1, ItemPrefix.None},
			new object[] {ItemType.IronPickaxe, 100, ItemPrefix.None},
			new object[] {ItemType.IronPickaxe, 100, ItemPrefix.Legendary}
		};
		
		[TestCaseSource(nameof(ItemSetDefaultsTestCases))]
		public void ItemSetDefaults_IsCorrect(ItemType type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSetDefaultsEventArgs> handler = (sender, e) =>
				{
					eventOccurred = true;
					Assert.AreEqual(terrariaItem, e.Item.WrappedItem);
				};
				itemService.ItemSetDefaults += handler;

				terrariaItem.SetDefaults((int)type);
				itemService.ItemSetDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCaseSource(nameof(ItemSetDefaultsTestCases))]
		public void ItemSetDefaults_OccursFromNetDefaults(ItemType type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSetDefaultsEventArgs> handler = (sender, e) => eventOccurred = true;
				itemService.ItemSetDefaults += handler;
				
				terrariaItem.netDefaults((int)type);
				itemService.ItemSetDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCase("Gold Broadsword")]
		public void ItemSetDefaults_OccursFromSetDefaultsString(string type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSetDefaultsEventArgs> handler = (sender, e) => eventOccurred = true;
				itemService.ItemSetDefaults += handler;

				terrariaItem.SetDefaults(type);
				itemService.ItemSetDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCaseSource(nameof(ItemSettingDefaultsTestCases))]
		public void ItemSettingDefaults_IsCorrect(ItemType type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSettingDefaultsEventArgs> handler = (sender, e) =>
				{
					eventOccurred = true;
					Assert.AreEqual(terrariaItem, e.Item.WrappedItem);
					Assert.AreEqual(type, e.Type);
				};
				itemService.ItemSettingDefaults += handler;

				terrariaItem.SetDefaults((int)type);
				itemService.ItemSettingDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCaseSource(nameof(ItemSettingDefaultsTestCases))]
		public void ItemSettingDefaults_ModifiesType(ItemType newType)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				EventHandler<ItemSettingDefaultsEventArgs> handler = (sender, e) => e.Type = newType;
				itemService.ItemSettingDefaults += handler;

				terrariaItem.SetDefaults();
				itemService.ItemSettingDefaults -= handler;
				
				Assert.AreEqual((int)newType, terrariaItem.netID);
			}
		}

		[TestCaseSource(nameof(ItemSettingDefaultsTestCases))]
		public void ItemSettingDefaults_Handled_StopsSetDefaults(ItemType type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				EventHandler<ItemSettingDefaultsEventArgs> handler = (sender, e) => e.Handled = true;
				itemService.ItemSettingDefaults += handler;

				terrariaItem.SetDefaults((int)type);
				itemService.ItemSettingDefaults -= handler;

				Assert.AreEqual(0, terrariaItem.netID, "SetDefaults should not have occurred.");
			}
		}

		[TestCaseSource(nameof(ItemSettingDefaultsTestCases))]

		public void ItemSettingDefaults_OccursFromNetDefaults(ItemType type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSettingDefaultsEventArgs> handler = (sender, e) => eventOccurred = true;
				itemService.ItemSettingDefaults += handler;

				terrariaItem.netDefaults((int)type);
				itemService.ItemSettingDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCase("Gold Broadsword")]
		public void ItemSettingDefaults_OccursFromSetDefaultsString(string type)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				var terrariaItem = new Terraria.Item();
				var eventOccurred = false;
				EventHandler<ItemSettingDefaultsEventArgs> handler = (sender, e) => eventOccurred = true;
				itemService.ItemSettingDefaults += handler;

				terrariaItem.SetDefaults(type);
				itemService.ItemSettingDefaults -= handler;

				Assert.IsTrue(eventOccurred);
			}
		}

		[TestCaseSource(nameof(CreateTestCases))]
		public void CreateItem_IsCorrect(ItemType type, int stack, ItemPrefix prefix)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				IItem item = itemService.CreateItem(type, stack, prefix);

				Assert.AreEqual(type, item.Type);
				Assert.AreEqual(stack, item.StackSize);
				Assert.AreEqual(prefix != ItemPrefix.None, item.Prefix != ItemPrefix.None);
			}
		}

		[TestCase(0)]
		[TestCase(1)]
		public void FindItems_Null_ReturnsAll(int populate)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				for (var i = 0; i < Terraria.Main.item.Length; ++i)
				{
					Terraria.Main.item[i] = new Terraria.Item {active = i < populate};
				}

				List<IItem> items = itemService.FindItems().ToList();

				Assert.AreEqual(populate, items.Count);
				for (var i = 0; i < populate; ++i)
				{
					Assert.AreSame(Terraria.Main.item[i], items[i].WrappedItem);
				}
			}
		}

		[TestCase(1)]
		public void FindItems_MultipleTimes_ReturnsSameInstance(int populate)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				for (var i = 0; i < Terraria.Main.item.Length; ++i)
				{
					Terraria.Main.item[i] = new Terraria.Item {active = i < populate};
				}

				List<IItem> items = itemService.FindItems().ToList();
				List<IItem> items2 = itemService.FindItems().ToList();
				
				for (var i = 0; i < populate; ++i)
				{
					Assert.AreSame(items[i], items2[i]);
				}
			}
		}

		[Test, TestCaseSource(nameof(FindTestCases))]
		public void FindItems_IsCorrect(Predicate<IItem> predicate)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				for (var i = 0; i < Terraria.Main.item.Length; ++i)
				{
					Terraria.Main.item[i] = new Terraria.Item {active = true, type = i};
				}

				IEnumerable<IItem> items = itemService.FindItems(predicate);

				foreach (IItem item in items)
				{
					Assert.IsTrue(predicate(item));
				}
			}
		}

		[TestCaseSource(nameof(SpawnTestCases))]
		public void SpawnItem_IsCorrect(ItemType type, int stack, ItemPrefix prefix)
		{
			using (var orion = new Orion())
			{
				var itemService = orion.GetService<ItemService>();
				IItem item = itemService.SpawnItem(type, new Vector2(1000, 2000), stack, prefix);

				Assert.AreEqual(type, item.Type);
				Assert.AreEqual(stack, item.StackSize);
				Assert.AreEqual(prefix != ItemPrefix.None, item.Prefix != ItemPrefix.None);
				Assert.That(item.Position.X, Is.InRange(900, 1100));
				Assert.That(item.Position.Y, Is.InRange(1900, 2100));
			}
		}
	}
}