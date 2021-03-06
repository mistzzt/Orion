﻿using Orion.Framework;
using OTAPI.Tile;

namespace Orion.World
{
	/// <summary>
	/// Provides access to Terraria's Main.tile mechanism, in which implementations may
	/// override the data source to and from the Terraria process.
	/// </summary>
	public interface ITileService : ISharedService, ITileCollection
	{
	}
}
