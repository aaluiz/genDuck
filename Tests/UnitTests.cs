using Contracts.Interfaces;
using NUnit.Framework;
using Services.Commands;
using Services.StartUp;

namespace Tests;

public class Tests
{
    ICommandLineUI? _commandLineUI;
    IGenStateService? _genStateService;
    IGenDuckService? _genDuckService;

    [SetUp]
    public void Setup()
    {
        _genDuckService = new GenDuckService();
        _genStateService = new GenStateService();
        _commandLineUI = new CommandLineUI(_genDuckService, _genStateService); 
    }

    [Test]
    public void CommandUI_Return1_Success()
    {
        string[] command = new string[] { "gen-duck" };
        var result = _commandLineUI!.ExecuteCommmand(command);
        Assert.IsTrue(result == 1);
    }
    
    [Test]
    public void CommandUI_Return0_Fail()
    {
        string[] command = new string[] { "gen-ducks" };
        var result = _commandLineUI!.ExecuteCommmand(command);
        Assert.IsTrue(result == -1);
    }

    [Test]
    public void CommandUI_GenState_Return1_Success()
    {
        string[] command = new string[] { "gen-states" };
        var result = _commandLineUI!.ExecuteCommmand(command);
        Assert.IsTrue(result == 1);
    }
    
    [Test]
    public void CommandUI_GenState_Return0_Fail()
    {
        string[] command = new string[] { "gen-statesx" };
        var result = _commandLineUI!.ExecuteCommmand(command);
        Assert.IsTrue(result == -1);
    }

    [Test]
    public void GenDuck_Return1_Success()
    {
        string[] command = new string[] { "gen-duck", "AuthState" };
        var result = _genDuckService!.Execute(command);
        Assert.IsTrue(result == 1);
    }

    [Test]
    public void GenDuck_Return0_Fail()
    {
        string[] command = new string[] { "gen-ducks" };
        var result = _genDuckService!.Execute(command);
        Assert.IsTrue(result == -1);
    }

    [Test]
    public void GenState_Return0_Success()
    {
        string[] command = new string[] { "gen-states" };
        var result = _genStateService!.Execute(command);
        Assert.IsTrue(result == 1);
    }

    [Test]
    public void GenState_Return0_Fail()
    {
        string[] command = new string[] { "gen-state" };
        var result = _genStateService!.Execute(command);
        Assert.IsTrue(result == -1);
    }
}