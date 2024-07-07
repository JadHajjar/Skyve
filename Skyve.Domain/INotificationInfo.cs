using System;
using System.Drawing;

namespace Skyve.Domain;
public interface INotificationInfo
{
	DateTime Time { get; }
	string Title { get; }
	string? Description { get; }
	string Icon { get; }
	Color? Color { get; }
	bool HasAction { get; }
	bool CanBeRead { get; }

	void OnRead();
	void OnClick();
	void OnRightClick();
}
