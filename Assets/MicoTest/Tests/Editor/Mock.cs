// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace MicoTest
{
    public class Mock : IMockId, IMockName
    {
        public int Id { get; }

        public string Name { get; set; }

        public Mock() : this(100)
        {
            Name = "Taro";
        }

        public Mock(int id)
        {
            Id = id;
            Name = "Ken";
        }
    }

    public interface IMockId
    {
        int Id { get; }
    }

    public interface IMockName
    {
        string Name { get; }
    }

    public class DerivedMock : Mock
    {
        public DerivedMock() : base(600)
        {
            Name = "DerivedMock";
        }
    }
}