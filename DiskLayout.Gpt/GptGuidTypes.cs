using DiskLayout.Domain.Enums;
using DiskLayout.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskLayout.Gpt
{
	public partial class GptLayoutDetector : IDiskLayoutDetector
	{
		private readonly Dictionary<Guid, PartitionAttributes> _guidAttributes = new()
		{
			[Guid.Parse("e3c9e316-0b5c-4db8-817d-f92df00215ae")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Microsoft Reserved

			[Guid.Parse("de94bba4-06d1-4d40-a16a-bfd50179d6ac")] = PartitionAttributes.Hidden | PartitionAttributes.ReadOnly | PartitionAttributes.NoAutomount, // Windows Recovery

			[Guid.Parse("5808c8aa-7e8f-42e0-85d2-e1e90434cfb3")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Windows LDM Metadata

			[Guid.Parse("af9b60a0-1431-4f62-bc68-3311714a69ad")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Windows LDM Data

			[Guid.Parse("e75caf8f-f680-4cee-afa3-b001e56efc2d")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Windows Storage Spaces

			[Guid.Parse("21686148-6449-6e6f-744e-656564454649")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // BIOS Boot (GRUB, Linux)

			[Guid.Parse("a19d880f-05fc-4d3b-a006-743f0f84911e")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Linux RAID

			[Guid.Parse("e6d6d379-f507-44c2-a23c-238f2a3df928")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Linux LVM

			[Guid.Parse("8da63339-0007-60c0-c436-083ac8230908")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Linux Reserved

			[Guid.Parse("55465300-0000-11aa-aa11-00306543ecac")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Apple Reserved

			[Guid.Parse("426f6f74-0000-11aa-aa11-00306543ecac")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Apple Boot

			[Guid.Parse("52414944-0000-11aa-aa11-00306543ecac")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Apple RAID

			[Guid.Parse("54494d45-0000-11aa-aa11-00306543ecac")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Apple Time Machine

			[Guid.Parse("c12a7328-f81f-11d2-ba4b-00a0c93ec93b")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // EFI System Partition (ESP)

			[Guid.Parse("fe3a2a5d-4f32-41a7-b725-accc3285a309")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // ChromeOS Kernel

			[Guid.Parse("3cb8e202-3b7e-47dd-8a3c-7ff2a13cfcec")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // ChromeOS RootFS

			[Guid.Parse("d3bfe2de-3daf-11df-ba40-e3a556d89593")] = PartitionAttributes.Hidden | PartitionAttributes.NoAutomount, // Intel Rapid Start
		};
    }
}
