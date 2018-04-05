using System;
using System.Collections.Generic;

namespace EF_Spike.DatabaseContext
{
    public partial class TblMembershipDetails
    {
        public int MembershipReference { get; set; }
        public short MembershipBenefitTypeReference { get; set; }
        public short MembershipTypeReference { get; set; }
        public int NumberOfMembers { get; set; }
        public int? NumberOfExcludedMembers { get; set; }
        public short? AverageAgeOfMembers { get; set; }

        public TblMembershipBenefitType MembershipBenefitTypeReferenceNavigation { get; set; }
        public TblMembership MembershipReferenceNavigation { get; set; }
        public TblMembershipType MembershipTypeReferenceNavigation { get; set; }
    }
}
