﻿using System;
using System.Collections.Generic;
using System.IO;
using Orion.Extensions;
using Orion.Framework;
using System.Threading.Tasks;

namespace Orion.Authorization
{
	/// <summary>
	/// Plain text account and group service which sources its account information from flat files in Orion's data
	/// subdirectory.
	/// </summary>
	[Service("Plain Text Account Service", Author = "Nyx Studios")]
	public class PlainTextAccountService : SharedService, IAccountService, IGroupService
	{
		/// <summary>
		/// The path to where <see cref="PlainTextUserAccount"/> objects are stored.
		/// </summary>
		public static string UserPathPrefix => Path.Combine("data", "users");

		/// <summary>
		/// The path to where <see cref="PlainTextGroup"/> objects are stored.
		/// </summary>
		public static string GroupPathPrefix => Path.Combine("data", "groups");

		/// <inheritdoc/>
		public PlainTextAccountService(Orion orion) : base(orion)
		{
			Directory.CreateDirectory(UserPathPrefix);
			Directory.CreateDirectory(GroupPathPrefix);
		}

		/// <inheritdoc/>
		public IEnumerable<IAccount> FindAccounts(Predicate<IAccount> predicate = null)
		{
			foreach (var filePath in Directory.GetFiles(UserPathPrefix, "*.ini"))
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					PlainTextUserAccount userAccount = new PlainTextUserAccount(this, fs);

					if (predicate == null)
					{
						yield return userAccount;
					}
					else if (predicate(userAccount))
					{
						yield return userAccount;
					}
				}
			}
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<IAccount>> FindAccountsAsync(Predicate<IAccount> predicate = null)
		{
			return await Task.Run(() => FindAccounts(predicate));
		}

		/// <inheritdoc/>
		public IAccount GetAccount(string accountName)
		{
			string accountPath = Path.Combine(UserPathPrefix, $"{accountName.Slugify()}.ini");

			if (!File.Exists(accountPath))
			{
				return default(IAccount);
			}

			using (FileStream fs = new FileStream(accountPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return new PlainTextUserAccount(this, fs);
			}
		}

		/// <inheritdoc/>
		public async Task<IAccount> GetAccountAsync(string accountName)
		{
			return await Task.Run(() => GetAccount(accountName));
		}

		/// <inheritdoc/>
		public IAccount AddAccount(string accountName)
		{
			PlainTextUserAccount userAccount;
			string accountPath;

			if (String.IsNullOrEmpty(accountName))
			{
				throw new ArgumentNullException(nameof(accountName));
			}

			accountPath = Path.Combine(UserPathPrefix, $"{accountName.Slugify()}.ini");

			if (File.Exists(accountPath))
			{
				throw new InvalidOperationException($"Account by the name of {accountName} already exists.");
			}

			userAccount = new PlainTextUserAccount(this)
			{
				AccountName = accountName
			};

			using (FileStream fs = new FileStream(accountPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
			{
				userAccount.ToStream(fs);
			}

			return userAccount;
		}

		/// <inheritdoc/>
		public async Task<IAccount> AddAccountAsync(string accountName)
		{
			return await Task.Run(() => AddAccount(accountName));
		}

		/// <inheritdoc/>
		public void DeleteAccount(IAccount user)
		{
			string accountPath;

			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			accountPath = Path.Combine(UserPathPrefix, $"{user.AccountName.Slugify()}.ini");

			File.Delete(accountPath);
		}

		/// <inheritdoc/>
		public async Task DeleteAccountAsync(IAccount user)
		{
			await Task.Run(() => DeleteAccount(user));
		}

		/// <inheritdoc/>
		public void SetPassword(IAccount userAccount, string password)
		{
			userAccount.SetPassword(password);
		}

		/// <inheritdoc/>
		public async Task SetPasswordAsync(IAccount userAccount, string password)
		{
			await userAccount.SetPasswordAsync(password);
		}

		/// <inheritdoc/>
		public void ChangePassword(IAccount userAccount, string currentPassword, string newPassword)
		{
			userAccount.ChangePassword(currentPassword, newPassword);
		}

		/// <inheritdoc/>
		public async Task ChangePasswordAsync(IAccount userAccount, string currentPassword, string newPassword)
		{
			await userAccount.ChangePasswordAsync(currentPassword, newPassword);
		}

		/// <inheritdoc/>
		public bool Authenticate(IAccount userAccount, string password)
		{
			return userAccount.Authenticate(password);
		}

		/// <inheritdoc/>
		public async Task<bool> AuthenticateAsync(IAccount userAccount, string password)
		{
			return await userAccount.AuthenticateAsync(password);
		}

		/// <inheritdoc/>
		public IGroup AdministratorGroup { get; }

		/// <inheritdoc/>
		public IGroup AnonymousGroup { get; }

		/// <inheritdoc/>
		public IEnumerable<IGroup> FindGroups(Predicate<IGroup> predicate = null)
		{
			foreach (var filePath in Directory.GetFiles(GroupPathPrefix, "*.ini"))
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					PlainTextGroup group = new PlainTextGroup(this, fs);

					if (predicate == null)
					{
						yield return group;
					}
					else if (predicate(group))
					{
						yield return group;
					}
				}
			}
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<IGroup>> FindGroupsAsync(Predicate<IGroup> predicate = null)
		{
			return await Task.Run(() => FindGroups(predicate));
		}

		/// <inheritdoc/>
		public IGroup AddGroup(string groupName, IEnumerable<IAccount> initialMembers = null)
		{
			PlainTextGroup group;
			string groupPath;

			if (String.IsNullOrEmpty(groupName))
			{
				throw new ArgumentNullException(nameof(groupName));
			}

			groupPath = Path.Combine(GroupPathPrefix, $"{groupName.Slugify()}.ini");

			if (File.Exists(groupPath))
			{
				throw new InvalidOperationException($"Group by the name of {groupName} already exists.");
			}

			group = new PlainTextGroup(this)
			{
				Name = groupName
			};

			if (initialMembers != null)
			{
				foreach (IAccount a in initialMembers)
				{
					group.AddMember(a);
				}
			}

			using (FileStream fs = new FileStream(groupPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
			{
				group.ToStream(fs);
			}

			return group;
		}

		/// <inheritdoc/>
		public async Task<IGroup> AddGroupAsync(string groupName, IEnumerable<IAccount> initialMembers = null)
		{
			return await Task.Run(() => AddGroup(groupName, initialMembers));
		}

		/// <inheritdoc/>
		public void DeleteGroup(IGroup group)
		{
			string groupPath = Path.Combine(GroupPathPrefix, $"{group.Name.Slugify()}.ini");

			File.Delete(groupPath);
		}

		/// <inheritdoc/>
		public async Task DeleteGroupAsync(IGroup group)
		{
			await Task.Run(() => DeleteGroup(group));
		}

		/// <inheritdoc/>
		public void AddMembers(IGroup group, params IAccount[] userAccounts)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public async Task AddMembersAsync(IGroup group, params IAccount[] userAccounts)
		{
			await Task.Run(() => AddMembers(group, userAccounts));
		}
	}
}
