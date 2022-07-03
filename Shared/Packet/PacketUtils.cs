﻿using System.Reflection;
using System.Runtime.InteropServices;
using Shared.Packet.Packets;

namespace Shared.Packet;

public static class PacketUtils {
    public static void SerializeHeaded<T>(Span<byte> data, PacketHeader header, T t) where T : struct, IPacket {
        header.Serialize(data);
        t.Serialize(data[Constants.HeaderSize..]);
    }

    public static T Deserialize<T>(Span<byte> data) where T : IPacket, new() {
        T packet = new T();
        packet.Deserialize(data);
        return packet;
    }

    public static int SizeOf<T>() where T : struct, IPacket {
        return Constants.HeaderSize + Marshal.SizeOf<T>();
    }

    public static void LogPacket<T>(T packet, string tag) where T : IPacket {
        if (packet is PlayerPacket or CapPacket) // These are too spammy
            return;

        Type packetType = packet.GetType();
        FieldInfo[] fields = packetType.GetFields();

        string prefix = $"{{{DateTime.Now}}} Debug";
        string msg = $"{prefix} [{tag}] {packetType.Name} {{\n";
        foreach (FieldInfo field in fields)
        {
            msg += $"{prefix}    {field.Name} = {field.GetValue(packet)}\n";
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(msg);
    }
    public static void LogPacketSame(string tag) {
        string prefix = $"{{{DateTime.Now}}} Debug";
        string msg = $"{prefix} [{tag}] {{same}}\n";

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(msg);
    }
}