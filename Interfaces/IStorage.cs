namespace UtilityBot.Interfaces;
internal interface IStorage
{
    Session GetSession(long chatId);

    int FuncCountingCharacters(string enteredText);

    decimal FuncSumNum(string enteredText);


}
