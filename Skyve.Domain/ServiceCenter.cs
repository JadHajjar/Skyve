﻿using Microsoft.Extensions.DependencyInjection;

using System;

namespace SkyveApp.Domain;

#nullable disable
public static class ServiceCenter
{
	public static IServiceProvider Provider { get; set; }

	public static T Get<T>()
	{
		if (Provider == null)
		{
			return default;
		}

		return Provider.GetService<T>();
	}

	public static T2 Get<T, T2>() where T2 : T
	{
		if (Provider == null)
		{
			return default;
		}

		return (T2)Provider.GetService<T>();
	}

	public static void Get<T1>(out T1 item1)
	{
		item1 = Get<T1>();
	}

	public static void Get<T1, T2>(out T1 item1, out T2 item2)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
	}

	public static void Get<T1, T2, T3>(out T1 item1, out T2 item2, out T3 item3)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
	}

	public static void Get<T1, T2, T3, T4>(out T1 item1, out T2 item2, out T3 item3, out T4 item4)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
	}

	public static void Get<T1, T2, T3, T4, T5>(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
		item5 = Get<T5>();
	}

	public static void Get<T1, T2, T3, T4, T5, T6>(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
		item5 = Get<T5>();
		item6 = Get<T6>();
	}

	public static void Get<T1, T2, T3, T4, T5, T6, T7>(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
		item5 = Get<T5>();
		item6 = Get<T6>();
		item7 = Get<T7>();
	}

	public static void Get<T1, T2, T3, T4, T5, T6, T7, T8>(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
		item5 = Get<T5>();
		item6 = Get<T6>();
		item7 = Get<T7>();
		item8 = Get<T8>();
	}

	public static void Get<T1, T2, T3, T4, T5, T6, T7, T8, T9>(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9)
	{
		item1 = Get<T1>();
		item2 = Get<T2>();
		item3 = Get<T3>();
		item4 = Get<T4>();
		item5 = Get<T5>();
		item6 = Get<T6>();
		item7 = Get<T7>();
		item8 = Get<T8>();
		item9 = Get<T9>();
	}
}
#nullable enable