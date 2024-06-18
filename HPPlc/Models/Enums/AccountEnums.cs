using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models.Enums
{
   enum IsPasswordSet
	{
		PasswordAssigned = 1,
		PasswordNotAssigned = 0
	}

	enum IsUserRegistered
	{
		Yes = 1,
		No = 0
	}

	enum ProfileStatus
	{
		New = 1,
		Existing = 2
	}
	enum StepsCompletted
	{
		Registration = 2,
		RegistrationOtp = 3,
		ExistingUserOtp = 4,
		SetPassword = 5
	}
}