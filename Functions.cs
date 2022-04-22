using System;

public class Functions
{
	public Functions()
	{
	}

	public bool IsNumber(string package)
	{
		Double number;
		bool isnumeric;
		
		isnumeric = Double.TryParse(package, out number);
		return isnumeric;
	}
	public bool IsEmpty(string str){
		if(String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str) )
			return true;
		return false;	
	}
}
