using System;

public class Beneficiary
{
    private string beneficairyID;

    public string BeneficiaryID
    {
        get { return beneficairyID; }
        set { beneficairyID = value; }
    }
    private string beneficiaryName;

    public string BeneficiaryName
    {
        get { return beneficiaryName; }
        set { beneficiaryName = value; }
    }

    private string beneficiaryBranch;

    public string BeneficiaryBranch
    {
        get { return beneficiaryBranch; }
        set { beneficiaryBranch = value; }
    }
    private string clientReference;

    public string ClientReference
    {
        get { return clientReference; }
        set { clientReference = value; }
    }



    public Beneficiary()
	{
	}
}
