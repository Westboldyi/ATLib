﻿namespace HeboTech.ATLib.Results
{
    public class ProductIdentificationInformation
    {
        public ProductIdentificationInformation(string information)
        {
            Information = information;
        }

        public string Information { get; }

        public override string ToString()
        {
            return Information;
        }
    }
}
