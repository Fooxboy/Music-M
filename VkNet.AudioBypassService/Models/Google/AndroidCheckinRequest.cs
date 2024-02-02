﻿// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: checkin.proto

using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace VkNet.AudioBypassService.Models.Google
{
	[ProtoContract]
	public class AndroidCheckinRequest : IExtensible
	{
		private string __pbn__DesiredBuild;

		private string __pbn__Digest;

		private string __pbn__Esn;

		private IExtension __pbn__extensionData;

		private int? __pbn__Fragment;

		private long? __pbn__Id;

		private string __pbn__Imei;

		private string __pbn__Locale;

		private long? __pbn__LoggingId;

		private string __pbn__MarketCheckin;

		private string __pbn__Meid;

		private ulong? __pbn__SecurityToken;

		private string __pbn__SerialNumber;

		private string __pbn__TimeZone;

		private string __pbn__UserName;

		private int? __pbn__UserSerialNumber;

		private int? __pbn__Version;

		[ProtoMember(1, Name = @"imei")]
		[DefaultValue("")]
		public string Imei
		{
			get => __pbn__Imei ?? "";
			set => __pbn__Imei = value;
		}

		[ProtoMember(10, Name = @"meid")]
		[DefaultValue("")]
		public string Meid
		{
			get => __pbn__Meid ?? "";
			set => __pbn__Meid = value;
		}

		[ProtoMember(9, Name = @"mac_addr")]
		public List<string> MacAddrs { get; } = new List<string>();

		[ProtoMember(19, Name = @"mac_addr_type")]
		public List<string> MacAddrTypes { get; } = new List<string>();

		[ProtoMember(16, Name = @"serial_number")]
		[DefaultValue("")]
		public string SerialNumber
		{
			get => __pbn__SerialNumber ?? "";
			set => __pbn__SerialNumber = value;
		}

		[ProtoMember(17, Name = @"esn")]
		[DefaultValue("")]
		public string Esn
		{
			get => __pbn__Esn ?? "";
			set => __pbn__Esn = value;
		}

		[ProtoMember(2, Name = @"id")]
		public long Id
		{
			get => __pbn__Id.GetValueOrDefault();
			set => __pbn__Id = value;
		}

		[ProtoMember(7, Name = @"logging_id")]
		public long LoggingId
		{
			get => __pbn__LoggingId.GetValueOrDefault();
			set => __pbn__LoggingId = value;
		}

		[ProtoMember(3, Name = @"digest")]
		[DefaultValue("")]
		public string Digest
		{
			get => __pbn__Digest ?? "";
			set => __pbn__Digest = value;
		}

		[ProtoMember(6, Name = @"locale")]
		[DefaultValue("")]
		public string Locale
		{
			get => __pbn__Locale ?? "";
			set => __pbn__Locale = value;
		}

		[ProtoMember(4, Name = @"checkin", IsRequired = true)]
		public AndroidCheckinProto Checkin { get; set; }

		[ProtoMember(5, Name = @"desired_build")]
		[DefaultValue("")]
		public string DesiredBuild
		{
			get => __pbn__DesiredBuild ?? "";
			set => __pbn__DesiredBuild = value;
		}

		[ProtoMember(8, Name = @"market_checkin")]
		[DefaultValue("")]
		public string MarketCheckin
		{
			get => __pbn__MarketCheckin ?? "";
			set => __pbn__MarketCheckin = value;
		}

		[ProtoMember(11, Name = @"account_cookie")]
		public List<string> AccountCookies { get; } = new List<string>();

		[ProtoMember(12, Name = @"time_zone")]
		[DefaultValue("")]
		public string TimeZone
		{
			get => __pbn__TimeZone ?? "";
			set => __pbn__TimeZone = value;
		}

		[ProtoMember(13, Name = @"security_token", DataFormat = DataFormat.FixedSize)]
		public ulong SecurityToken
		{
			get => __pbn__SecurityToken.GetValueOrDefault();
			set => __pbn__SecurityToken = value;
		}

		[ProtoMember(14, Name = @"version")]
		public int Version
		{
			get => __pbn__Version.GetValueOrDefault();
			set => __pbn__Version = value;
		}

		[ProtoMember(15, Name = @"ota_cert")]
		public List<string> OtaCerts { get; } = new List<string>();

		[ProtoMember(20, Name = @"fragment")]
		public int Fragment
		{
			get => __pbn__Fragment.GetValueOrDefault();
			set => __pbn__Fragment = value;
		}

		[ProtoMember(21, Name = @"user_name")]
		[DefaultValue("")]
		public string UserName
		{
			get => __pbn__UserName ?? "";
			set => __pbn__UserName = value;
		}

		[ProtoMember(22, Name = @"user_serial_number")]
		public int UserSerialNumber
		{
			get => __pbn__UserSerialNumber.GetValueOrDefault();
			set => __pbn__UserSerialNumber = value;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);
		}

		public bool ShouldSerializeImei()
		{
			return __pbn__Imei != null;
		}

		public void ResetImei()
		{
			__pbn__Imei = null;
		}

		public bool ShouldSerializeMeid()
		{
			return __pbn__Meid != null;
		}

		public void ResetMeid()
		{
			__pbn__Meid = null;
		}

		public bool ShouldSerializeSerialNumber()
		{
			return __pbn__SerialNumber != null;
		}

		public void ResetSerialNumber()
		{
			__pbn__SerialNumber = null;
		}

		public bool ShouldSerializeEsn()
		{
			return __pbn__Esn != null;
		}

		public void ResetEsn()
		{
			__pbn__Esn = null;
		}

		public bool ShouldSerializeId()
		{
			return __pbn__Id != null;
		}

		public void ResetId()
		{
			__pbn__Id = null;
		}

		public bool ShouldSerializeLoggingId()
		{
			return __pbn__LoggingId != null;
		}

		public void ResetLoggingId()
		{
			__pbn__LoggingId = null;
		}

		public bool ShouldSerializeDigest()
		{
			return __pbn__Digest != null;
		}

		public void ResetDigest()
		{
			__pbn__Digest = null;
		}

		public bool ShouldSerializeLocale()
		{
			return __pbn__Locale != null;
		}

		public void ResetLocale()
		{
			__pbn__Locale = null;
		}

		public bool ShouldSerializeDesiredBuild()
		{
			return __pbn__DesiredBuild != null;
		}

		public void ResetDesiredBuild()
		{
			__pbn__DesiredBuild = null;
		}

		public bool ShouldSerializeMarketCheckin()
		{
			return __pbn__MarketCheckin != null;
		}

		public void ResetMarketCheckin()
		{
			__pbn__MarketCheckin = null;
		}

		public bool ShouldSerializeTimeZone()
		{
			return __pbn__TimeZone != null;
		}

		public void ResetTimeZone()
		{
			__pbn__TimeZone = null;
		}

		public bool ShouldSerializeSecurityToken()
		{
			return __pbn__SecurityToken != null;
		}

		public void ResetSecurityToken()
		{
			__pbn__SecurityToken = null;
		}

		public bool ShouldSerializeVersion()
		{
			return __pbn__Version != null;
		}

		public void ResetVersion()
		{
			__pbn__Version = null;
		}

		public bool ShouldSerializeFragment()
		{
			return __pbn__Fragment != null;
		}

		public void ResetFragment()
		{
			__pbn__Fragment = null;
		}

		public bool ShouldSerializeUserName()
		{
			return __pbn__UserName != null;
		}

		public void ResetUserName()
		{
			__pbn__UserName = null;
		}

		public bool ShouldSerializeUserSerialNumber()
		{
			return __pbn__UserSerialNumber != null;
		}

		public void ResetUserSerialNumber()
		{
			__pbn__UserSerialNumber = null;
		}
	}
}
