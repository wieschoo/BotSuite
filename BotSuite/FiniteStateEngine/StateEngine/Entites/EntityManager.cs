// -----------------------------------------------------------------------
//  <copyright file="EntityManager.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.FiniteStateEngine.StateEngine.Entites
{
	using System.Collections.Generic;

	/// <summary>
	///     The entity manager.
	/// </summary>
	public class EntityManager
	{
		/// <summary>
		///     The dic entites.
		/// </summary>
		private readonly Dictionary<int, BaseEntity> dicEntites;

		/// <summary>
		///     Initializes a new instance of the <see cref="EntityManager" /> class.
		/// </summary>
		public EntityManager()
		{
			this.dicEntites = new Dictionary<int, BaseEntity>();
		}

		/// <summary>
		///     The register entity.
		/// </summary>
		/// <param name="entity">
		///     The entity.
		/// </param>
		public void RegisterEntity(BaseEntity entity)
		{
			this.dicEntites.Add(entity.Id, entity);
		}

		/// <summary>
		///     The remove entity.
		/// </summary>
		/// <param name="entity">
		///     The entity.
		/// </param>
		public void RemoveEntity(BaseEntity entity)
		{
			if (this.dicEntites.ContainsKey(entity.Id))
			{
				this.dicEntites.Remove(entity.Id);
			}
		}

		/// <summary>
		///     The get entity by id.
		/// </summary>
		/// <param name="id">
		///     The id.
		/// </param>
		/// <returns>
		///     The <see cref="BaseEntity" />.
		/// </returns>
		public BaseEntity GetEntityById(int id)
		{
			if (this.dicEntites.ContainsKey(id))
			{
				return this.dicEntites[id];
			}

			return null;
		}
	}
}