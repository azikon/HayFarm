public struct UserExperienceChangeSignal
{
    public int AddExperience { get; set; }

    public UserExperienceChangeSignal( int addExperience )
    {
        AddExperience = addExperience;
    }
}