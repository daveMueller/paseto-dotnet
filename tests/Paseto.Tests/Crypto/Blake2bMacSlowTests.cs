﻿namespace Paseto.Tests.Crypto;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Paseto.Cryptography;
using Xunit;

/// <summary>
/// Tests that use the RFC slow implementation to assert fixed byte arrays
/// </summary>
/// <remarks>
/// These tests assert a baseline - they let us prove our crappy implementation
/// which we can then use to assert better implementations
/// </remarks>
public class Blake2bMacSlowTests
{
    [Fact]
    public void CorrectlyComputesShortHash512()
    {
        var bytes = Encoding.UTF8.GetBytes("ShortSign");
        byte[] expected = new byte[] {
                0xb7, 0x65, 0xda, 0xf6, 0x65, 0x11, 0x43, 0x46, 0x56, 0x74, 0x9e, 0xd1, 0x45, 0xa2, 0x02, 0xe6,
                0x65, 0x06, 0x7a, 0xce, 0xa1, 0xff, 0x60, 0x8c, 0x4b, 0xbd, 0xb8, 0x28, 0x4a, 0x28, 0x7e, 0x26,
                0xe1, 0x83, 0xca, 0x3b, 0xcf, 0x26, 0x13, 0x5a, 0x8b, 0xea, 0x89, 0x47, 0x8d, 0x79, 0x7e, 0x94,
                0x64, 0x0f, 0xbe, 0x4b, 0x42, 0x40, 0x24, 0x7c, 0x48, 0xba, 0x86, 0xbe, 0x34, 0xf2, 0x61, 0xa6
            };

        byte[] actual = ComputeHash(bytes, _key512, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesHash256With512BitKey()
    {
        byte[] data = new byte[] {
                0xfb, 0xfc, 0xc9, 0xe9, 0x2a, 0x38, 0x14, 0x24, 0x04, 0x3a, 0x02, 0x3d, 0xef, 0xad, 0xbf, 0xe8,
                0xa4, 0xe7, 0xf5, 0x7b, 0x0f, 0xcb, 0x93, 0x25, 0xa9, 0xc1, 0x0c, 0x7c, 0xa3, 0x70, 0x32, 0xa6,
                0xda, 0x1a, 0x60, 0xd8, 0xbb, 0x42, 0xd5, 0xb5, 0x79, 0x9e, 0x0d, 0xb1, 0x07, 0xd6, 0x4b, 0xb5,
                0xa6, 0xdc, 0x6a, 0x94, 0x3c, 0xb2, 0x7c, 0x04, 0x52, 0xba, 0x9f, 0x53, 0x90, 0x58, 0xac, 0xb3,
                0xd5, 0x92, 0x67, 0x23, 0xe3, 0x7d, 0x8a, 0x8f, 0xae, 0x08, 0x06, 0x8c, 0xdc, 0x06, 0xed, 0xa3,
                0xf1, 0xe0, 0x7d, 0x6f, 0x31, 0x5f, 0xfe, 0x9f, 0x18
            };

        byte[] expected = new byte[] {
                0x8d, 0x58, 0xfa, 0xa5, 0x84, 0x26, 0x69, 0xc4, 0x1f, 0x75, 0xd0, 0x28, 0x8a, 0x14, 0xfa, 0x86,
                0xe3, 0x18, 0xed, 0x9d, 0xef, 0x39, 0x04, 0xb9, 0x98, 0x35, 0xe7, 0xdb, 0x81, 0x2e, 0x44, 0x16
            };

        byte[] actual = ComputeHash(data, _key512, 256);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesHash256With128BitKey()
    {
        byte[] key = new byte[] {
                0x80, 0xff, 0x75, 0x12, 0x55, 0xa9, 0xce, 0x5a, 0xd7, 0x0e, 0xa0, 0x72, 0x73, 0x83, 0xfe, 0x01
            };

        byte[] data = new byte[] {
                0xfb, 0xfc, 0xc9, 0xe9, 0x2a, 0x38, 0x14, 0x24, 0x04, 0x3a, 0x02, 0x3d, 0xef, 0xad, 0xbf, 0xe8,
                0xa4, 0xe7, 0xf5, 0x7b, 0x0f, 0xcb, 0x93, 0x25, 0xa9, 0xc1, 0x0c, 0x7c, 0xa3, 0x70, 0x32, 0xa6,
                0xda, 0x1a, 0x60, 0xd8, 0xbb, 0x42, 0xd5, 0xb5, 0x79, 0x9e, 0x0d, 0xb1, 0x07, 0xd6, 0x4b, 0xb5,
                0xa6, 0xdc, 0x6a, 0x94, 0x3c, 0xb2, 0x7c, 0x04, 0x52, 0xba, 0x9f, 0x53, 0x90, 0x58, 0xac, 0xb3,
                0xd5, 0x92, 0x67, 0x23, 0xe3, 0x7d, 0x8a, 0x8f, 0xae, 0x08, 0x06, 0x8c, 0xdc, 0x06, 0xed, 0xa3,
                0xf1, 0xe0, 0x7d, 0x6f, 0x31, 0x5f, 0xfe, 0x9f, 0x18
            };

        byte[] expected = new byte[] {
                0x14, 0x7f, 0xa8, 0xda, 0xf7, 0x7a, 0xcb, 0x67, 0x42, 0xaa, 0xaf, 0x97, 0x60, 0x4a, 0xf8, 0xec,
                0x8a, 0x65, 0xf1, 0xb5, 0xc1, 0x92, 0xd7, 0x85, 0x63, 0xbd, 0xcb, 0x82, 0x9e, 0x7c, 0xa2, 0x1c
            };

        byte[] actual = ComputeHash(data, key, 256);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesHash256With16BitKey()
    {
        byte[] key = new byte[] {
                0x61, 0x62
            };

        byte[] data = new byte[] {
                0xfb, 0xfc, 0xc9, 0xe9, 0x2a, 0x38, 0x14, 0x24, 0x04, 0x3a, 0x02, 0x3d, 0xef, 0xad, 0xbf, 0xe8,
                0xa4, 0xe7, 0xf5, 0x7b, 0x0f, 0xcb, 0x93, 0x25, 0xa9, 0xc1, 0x0c, 0x7c, 0xa3, 0x70, 0x32, 0xa6,
                0xda, 0x1a, 0x60, 0xd8, 0xbb, 0x42, 0xd5, 0xb5, 0x79, 0x9e, 0x0d, 0xb1, 0x07, 0xd6, 0x4b, 0xb5,
                0xa6, 0xdc, 0x6a, 0x94, 0x3c, 0xb2, 0x7c, 0x04, 0x52, 0xba, 0x9f, 0x53, 0x90, 0x58, 0xac, 0xb3,
                0xd5, 0x92, 0x67, 0x23, 0xe3, 0x7d, 0x8a, 0x8f, 0xae, 0x08, 0x06, 0x8c, 0xdc, 0x06, 0xed, 0xa3,
                0xf1, 0xe0, 0x7d, 0x6f, 0x31, 0x5f, 0xfe, 0x9f, 0x18
            };

        byte[] expected = new byte[] {
                  0x00, 0x75, 0x0d, 0xac, 0x05, 0xe0, 0x87, 0xa5, 0x53, 0xb9, 0xbc, 0x61, 0x67, 0x4a, 0x40, 0x99,
                  0x88, 0xa5, 0xd9, 0x5c, 0xc8, 0x1d, 0xdb, 0xae, 0x63, 0x16, 0xcd, 0x27, 0x48, 0x24, 0x61, 0xa7
            };

        byte[] actual = ComputeHash(data, key, 256);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesExactBoundaryHash512()
    {
        byte[] data = new byte[] {
                0x36, 0x05, 0xb9, 0xb9, 0xdf, 0x63, 0xa9, 0x62, 0xb8, 0xa8, 0xb7, 0x72, 0x96, 0xaf, 0x61, 0xd3,
                0x7e, 0xfd, 0x22, 0xf6, 0xaa, 0xba, 0x25, 0xa3, 0x03, 0x86, 0x7d, 0xd9, 0x68, 0x69, 0x7e, 0xc5,
                0xd0, 0x81, 0xe2, 0x95, 0x47, 0xfb, 0x90, 0x1b, 0xbd, 0x4f, 0x2e, 0x1a, 0xcc, 0x19, 0xef, 0x28,
                0x88, 0x29, 0x2f, 0xa9, 0x62, 0xaf, 0xad, 0x53, 0x50, 0xbf, 0x8b, 0x1d, 0x5e, 0x26, 0xe7, 0x49
            };

        byte[] expected = new byte[] {
                0xc8, 0x8c, 0x60, 0xcc, 0x0d, 0x70, 0xcc, 0x44, 0xba, 0xf0, 0xd2, 0x7f, 0xf6, 0xb7, 0xa1, 0xf5,
                0x81, 0xb6, 0x54, 0x79, 0x80, 0x39, 0xf2, 0x9a, 0xd4, 0xc5, 0x28, 0x91, 0x5e, 0xf7, 0x13, 0x46,
                0x41, 0x4c, 0xd2, 0xc0, 0x5e, 0x38, 0x36, 0x25, 0x9b, 0x3d, 0x7f, 0xb5, 0xda, 0x04, 0x1d, 0x18,
                0x62, 0xc3, 0x2c, 0x96, 0x20, 0x55, 0xdb, 0xd1, 0x68, 0xd2, 0xdd, 0x65, 0xed, 0xac, 0x7c, 0x95
            };

        byte[] actual = ComputeHash(data, _key512, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesLongNonBoundary512()
    {
        byte[] data = new byte[] {
                0x67, 0x8e, 0x9f, 0x40, 0x28, 0x3e, 0x42, 0x13, 0xe1, 0xda, 0x24, 0xc8, 0x82, 0xe3, 0x31, 0x2d,
                0xc7, 0x20, 0x09, 0xb3, 0xc7, 0x97, 0x9a, 0xa6, 0x0a, 0xc1, 0x1a, 0xcc, 0x56, 0xb6, 0x41, 0xa1,
                0x55, 0x58, 0x08, 0x31, 0x47, 0x6d, 0x92, 0x65, 0xfc, 0x9a, 0x93, 0x3e, 0x0d, 0xd9, 0xdf, 0x39,
                0x29, 0xda, 0xc3, 0x73, 0x4e, 0xb9, 0x0b, 0x70, 0x07, 0x49, 0x00, 0x56, 0x5e, 0xb6, 0x31, 0x16,
                0x20, 0x50, 0xdc, 0x6b, 0xb1, 0x96, 0xf8, 0x0f, 0xc7, 0x10, 0xae, 0x4e, 0x44, 0x7f, 0xea, 0xb5,
                0x5d, 0xd3, 0xe1, 0xd8, 0xe6, 0x26, 0x4b, 0xa0, 0xd6, 0x46, 0xfb, 0x41, 0x1a, 0x74, 0xa4, 0xca,
                0x99, 0xe4, 0x15, 0xf2, 0x27, 0x32, 0xd7, 0x6e, 0x41, 0x5e, 0x75, 0x91, 0xbc, 0xf4, 0xed, 0x96,
                0x64, 0x06, 0x1e, 0x71, 0x83, 0xfe, 0x3c, 0x2c, 0x50, 0xe7, 0x14, 0x7c, 0xf0, 0x09, 0xbd, 0xd3,
                0x77, 0xc2, 0x22, 0x13, 0x81, 0xbb, 0xbf, 0x0e, 0xc0, 0xaa, 0xa1, 0x8a, 0x4e, 0xbf, 0xba, 0x7c,
                0x10, 0xf7, 0xc5, 0x50, 0xd1, 0x6c, 0x86, 0xd6, 0x99, 0xb7, 0xbc, 0xe7, 0x36, 0x2e, 0x6a, 0x70,
                0xba, 0x33
            };

        byte[] expected = new byte[] {
                0x8b, 0x66, 0x12, 0xbc, 0x61, 0x0f, 0x4b, 0xa1, 0x58, 0x2a, 0x67, 0xb1, 0xd2, 0x52, 0xbb, 0xc5,
                0x2a, 0xd7, 0xd8, 0xce, 0xaa, 0xd0, 0xe7, 0xd5, 0x60, 0x6f, 0x78, 0xd9, 0xef, 0x77, 0xf8, 0xf1,
                0x21, 0xdc, 0xd2, 0x43, 0x47, 0x8d, 0x6c, 0xc2, 0x10, 0xfc, 0xe7, 0x49, 0x84, 0x52, 0x32, 0x5c,
                0xf7, 0x84, 0xf5, 0xd2, 0x2d, 0xd1, 0xce, 0x5f, 0x59, 0xb5, 0x05, 0xc1, 0x59, 0x66, 0xb0, 0x04
            };

        byte[] actual = ComputeHash(data, _key512, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesLongBoundaryAligned512()
    {
        byte[] data = new byte[] {
                0xb4, 0x49, 0xfe, 0xde, 0x26, 0xe3, 0xee, 0x7c, 0x08, 0x6f, 0xfa, 0x4f, 0xe9, 0xbd, 0x36, 0xa9,
                0xdf, 0x70, 0xb6, 0xb2, 0x39, 0x17, 0x6b, 0x2c, 0x5f, 0x14, 0x26, 0x99, 0x65, 0x56, 0xb3, 0x09,
                0x80, 0x6b, 0x18, 0xfe, 0x4a, 0x2f, 0x7f, 0xb8, 0x90, 0x7c, 0x8b, 0xb5, 0xe8, 0xf4, 0x12, 0x3e,
                0xa7, 0xe0, 0xd2, 0x30, 0xbe, 0x68, 0x9f, 0xdf, 0x4f, 0x85, 0xa2, 0xf5, 0xff, 0x36, 0x8b, 0x65,
                0x30, 0x98, 0xdd, 0x77, 0x5c, 0x97, 0x68, 0x43, 0x4d, 0xa0, 0xbc, 0x23, 0x7b, 0xb9, 0x10, 0x1d,
                0xae, 0x37, 0x51, 0xa5, 0xf5, 0xe0, 0x0a, 0xa5, 0x4c, 0xab, 0x02, 0x75, 0x4a, 0x0e, 0xd0, 0x8a,
                0xd2, 0xf0, 0xea, 0x4b, 0x09, 0x33, 0xcb, 0x9c, 0x75, 0x48, 0x37, 0x6e, 0x0c, 0xde, 0x2e, 0x3e,
                0x77, 0x74, 0x31, 0xca, 0x73, 0x28, 0x6d, 0xfb, 0xcc, 0x5a, 0x46, 0x8b, 0xf8, 0xc8, 0xcf, 0x2d,
                0xf7, 0x3f, 0x38, 0x9f, 0x38, 0x75, 0x9c, 0xc0, 0xda, 0x28, 0xdf, 0xbc, 0x1a, 0x28, 0x1d, 0x43,
                0x89, 0xae, 0x6f, 0xd4, 0x30, 0x57, 0xf0, 0x12, 0x70, 0xec, 0x34, 0xf4, 0x8a, 0x12, 0x64, 0xa6
            };

        byte[] expected = new byte[] {
                0xad, 0xb8, 0xac, 0x95, 0x87, 0x45, 0x82, 0x99, 0x82, 0x93, 0x5e, 0xfa, 0xf5, 0x16, 0x83, 0x18,
                0xe2, 0x9f, 0xcd, 0xb0, 0x5b, 0x2c, 0x8d, 0x5e, 0x2e, 0x24, 0x9f, 0xd8, 0x90, 0xb2, 0x61, 0xd1,
                0x15, 0xa8, 0x2e, 0x33, 0x31, 0x72, 0x81, 0x05, 0x3d, 0x91, 0x0e, 0xae, 0xe4, 0x5a, 0xef, 0xae,
                0x2e, 0x31, 0x2a, 0x21, 0x21, 0x34, 0x7c, 0xe9, 0x07, 0xbc, 0x04, 0x65, 0xc7, 0xa8, 0xc6, 0xaf
            };

        byte[] actual = ComputeHash(data, _key512, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesLongBoundary512WithNonBoundaryKey()
    {
        byte[] key = new byte[] {
                0x75, 0x6d, 0x97, 0xd3, 0x47, 0x79, 0xe0, 0x6e, 0x6b, 0x38, 0x23, 0x80, 0x6e, 0x63, 0x3a, 0xa4,
                0xf6, 0x08, 0xf8, 0xb3, 0xd9, 0x85, 0x9e, 0x97, 0xad, 0x38, 0x74, 0x11, 0x35, 0x9f, 0x6f, 0xeb,
                0x9b, 0xe4, 0x4c, 0xd9, 0x3a, 0x64, 0xfa, 0x28, 0x61, 0xd0, 0xe0, 0x48, 0x68, 0xc4, 0x1a
            };

        byte[] data = new byte[] {
                0xc6, 0x24, 0x0a, 0xa2, 0x5a, 0x4e, 0x89, 0x6d, 0x06, 0x8f, 0x69, 0x96, 0xd3, 0xde, 0x7f, 0x3e,
                0x30, 0xc4, 0x22, 0xdd, 0xdb, 0xee, 0xf5, 0x59, 0xac, 0x64, 0xf8, 0x75, 0x9e, 0x72, 0x9c, 0x6c,
                0x39, 0xc8, 0xe8, 0x0c, 0xe2, 0x6b, 0xc0, 0x8c, 0x35, 0xde, 0x79, 0xe1, 0xe4, 0xe6, 0x5e, 0x59,
                0x57, 0x8c, 0x08, 0xcf, 0x99, 0x49, 0x77, 0x7d, 0x70, 0x46, 0xa0, 0x74, 0x05, 0x29, 0xfa, 0xaf
            };

        byte[] expected = new byte[] {
                0xf8, 0x4e, 0xf4, 0x61, 0x40, 0x55, 0x99, 0x0c, 0x08, 0xcf, 0xce, 0x78, 0x53, 0x04, 0xc0, 0x2e,
                0x54, 0xbd, 0x3e, 0xca, 0x0e, 0x90, 0xdd, 0xcc, 0x31, 0xda, 0x16, 0x1e, 0x4e, 0x12, 0xb7, 0xda,
                0xc6, 0xff, 0x47, 0x62, 0x2f, 0x9e, 0xaa, 0x7b, 0xbf, 0x67, 0xde, 0xad, 0xa6, 0x05, 0x0a, 0xb0,
                0xd1, 0xd4, 0x14, 0x0e, 0xec, 0xcb, 0x4c, 0x2d, 0xdf, 0x54, 0x91, 0x7e, 0xd5, 0x1e, 0x31, 0xc1
            };

        byte[] actual = ComputeHash(data, key, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CorrectlyComputesLongNonBoundary512WithNonBoundaryKey()
    {
        byte[] key = new byte[] {
                0xf0, 0x12, 0x4a, 0x69, 0xdc, 0x49, 0x39, 0xa7, 0x1e, 0xe8, 0x74, 0x55, 0x78, 0x5f, 0xd7, 0x9f,
                0xcd, 0xee, 0x88, 0x17, 0x14, 0xe0, 0x2e, 0xeb, 0xfa, 0x63, 0x4c, 0x69, 0x55, 0x67, 0xda, 0x6f
            };

        byte[] data = new byte[] {
                0xfb, 0xfc, 0xc9, 0xe9, 0x2a, 0x38, 0x14, 0x24, 0x04, 0x3a, 0x02, 0x3d, 0xef, 0xad, 0xbf, 0xe8,
                0xa4, 0xe7, 0xf5, 0x7b, 0x0f, 0xcb, 0x93, 0x25, 0xa9, 0xc1, 0x0c, 0x7c, 0xa3, 0x70, 0x32, 0xa6,
                0xda, 0x1a, 0x60, 0xd8, 0xbb, 0x42, 0xd5, 0xb5, 0x79, 0x9e, 0x0d, 0xb1, 0x07, 0xd6, 0x4b, 0xb5,
                0xa6, 0xdc, 0x6a, 0x94, 0x3c, 0xb2, 0x7c, 0x04, 0x52, 0xba, 0x9f, 0x53, 0x90, 0x58, 0xac, 0xb3,
                0xd5, 0x92, 0x67, 0x23, 0xe3, 0x7d, 0x8a, 0x8f, 0xae, 0x08, 0x06, 0x8c, 0xdc, 0x06, 0xed, 0xa3,
                0xf1, 0xe0, 0x7d, 0x6f, 0x31, 0x5f, 0xfe, 0x9f, 0x18
            };

        byte[] expected = new byte[] {
                0xac, 0x04, 0x88, 0x3b, 0x68, 0xf3, 0x82, 0x34, 0xc0, 0xc8, 0x54, 0x47, 0xa6, 0x69, 0x68, 0x26,
                0x86, 0x75, 0x95, 0xa0, 0x50, 0xd9, 0x4f, 0x69, 0xad, 0x19, 0x5f, 0x56, 0x34, 0xc1, 0x6e, 0x72,
                0x5f, 0x78, 0xab, 0x34, 0xda, 0x21, 0x34, 0xad, 0xea, 0x01, 0x76, 0xad, 0x1c, 0x26, 0x67, 0x3a,
                0xa5, 0x5d, 0x8e, 0x25, 0xa0, 0xd0, 0xe4, 0xbc, 0xf8, 0xe7, 0x3e, 0x50, 0x0d, 0xcd, 0x3e, 0x4c
            };

        byte[] actual = ComputeHash(data, key, 512);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EmptyMessageWithKey()
    {
        var expected = new byte[]
        {
                0x91, 0xcc, 0x35, 0xfc, 0x51, 0xce, 0x73, 0x4e, 0xae, 0x9e, 0x57, 0xa2, 0x07, 0x25, 0xd6, 0x80,
                0x62, 0xb0, 0xbd, 0x6d, 0xe1, 0x96, 0x5a, 0x4b, 0x5d, 0xc5, 0xeb, 0x4d, 0x60, 0x40, 0x2d, 0x02,
                0xd0, 0x0b, 0x50, 0x79, 0xb0, 0x90, 0x77, 0x75, 0xe3, 0x17, 0xfd, 0x84, 0xa1, 0x49, 0x63, 0x42,
                0x53, 0xc9, 0xd1, 0xfd, 0x01, 0x81, 0x9e, 0x20, 0x27, 0x29, 0xaf, 0xfb, 0xf4, 0x7b, 0x00, 0xe5
        };

        byte[] actual = ComputeHash(Array.Empty<byte>(), Encoding.UTF8.GetBytes("abc"), 512);
        Assert.Equal(expected, actual);
    }

    private static byte[] _key512 = new byte[] {
            0x45, 0xa7, 0x31, 0xa3, 0x92, 0xcb, 0xd5, 0x0e, 0xd3, 0xe2, 0x16, 0xcc, 0xe0, 0x34, 0x2b, 0x72,
            0x9b, 0x7f, 0x75, 0x64, 0xcf, 0x8a, 0x16, 0xf3, 0x34, 0x3d, 0xc3, 0xeb, 0xa6, 0x88, 0x0e, 0x7a,
            0xef, 0x0d, 0xef, 0x5c, 0xcd, 0x1d, 0x8e, 0x3a, 0x38, 0x37, 0xd0, 0x63, 0x80, 0x6e, 0xcb, 0xc2,
            0x89, 0x5d, 0x39, 0xe4, 0xe9, 0xd9, 0x95, 0x8f, 0x97, 0xea, 0xd6, 0xab, 0x7d, 0x20, 0xc3, 0x16
        };

    [SuppressMessage("Microsoft.Usage", "CA1801")]
    private static void PrintHash(byte[] data)
    {
#if DEBUG
        int byteOffset = 0;
        System.Console.WriteLine("#HASH:");
        while (byteOffset < data.Length)
        {
            int count = Math.Min(16, (data.Length - byteOffset));
            for (int i = 0; i < count; ++i)
                System.Console.Write($"{data[byteOffset + i]:X2} ");
            System.Console.WriteLine();

            byteOffset += count;
        }
#endif
    }

    private static byte[] ComputeHash(byte[] data, byte[] key, int hashSize)
    {
        Blake2bMac hmac = new Blake2bMac(key, hashSize, () => new Blake2bSlow(hashSize / 8));
        hmac.Initialize();

        var result = hmac.ComputeHash(data);

        PrintHash(result);
        return result;
    }
}
