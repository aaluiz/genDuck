using LanguageExt;
using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static System.Console;
using static System.IO.Path;
using static System.Reflection.Assembly;
using static Tools.Business.GenDucks;
using static Tools.Business.WriteDuck;

namespace Tests
{
    [TestFixture]
    public class TestingFunctions
    {
        [Test]
        public void GenerateDuckFile_ReturnDuckFileSucess()
        {
            var result = GenerateDuckFile("AuthState").GetEitherObject().Result;

            Assert.IsNotNull(result);
        }

        [Test]
        public void GenerateStateFile_ReturnFileContent()
        {
            string currentPath = GetCurrentDirectory();
            var result = GenerateStateFile(currentPath).GetEitherObject().Result.ToList();

            Assert.IsTrue(result.Count == 2);
        }

        [Test]
        public void GetFilesNamesFromDirectory_ReturnTwoFiles()
        {
            var resulFilesNames = GetFilesNamesFromDirectory(GetCurrentDirectory());

            List<string> result = new();

            resulFilesNames.Match(
                Left: x => WriteLine(x.Message),
                Right: y => result = y.ToList()
                );

            Assert.AreEqual(2, result.Count);

        }

        [Test]
        public void GetPropertiesFromState_ReturnString()
        {
            string json = @"
{
  'token': 'any',
  'validToken': 'boolean',
  'profile?': 'Profile | null',
  'logged': 'boolean',
  'expires': 'Date'
}
";
            var result = GetPropertiesFromState(json);

            Assert.IsTrue(result.Contains(@"  token:any;"));
        }

        [Test]
        public void WriteDuck_ReturnImports()
        {
            string imports = SetUpImports("AuthState");

            Assert.AreEqual(@"import {
  createAction,
  props,
  createReducer,
  on,
  createFeatureSelector
} from '@ngrx/store';
import { AuthState as State } from './AuthState.model';

// actions", imports);
        }

        [Test]
        public void CreateAction_ReturnActonCode()
        {
            string result = CreateAction("token", "any", "Auth");

            Assert.AreEqual(@"export const setToken = createAction(
  '[AUTH] SET_TOKEN',
  props<{ token: any }>()
);
", result);
        }

        [Test]
        public void CreateReducer_ReturnReducer()
        {
            var inputs = new List<string>() { "token", "validToken", "profile", "userData", "logged", "expires"}.ToImmutableList();

            var result = CreateReducer( "Auth", inputs);

            Assert.AreEqual(@"// reducer
export const authReducer = createReducer(
  initialState,
  on(setToken, (state, { token }): State => ({ ...state, token })),
  on(setValidToken, (state, { validToken }): State => ({ ...state, validToken })),
  on(setProfile, (state, { profile }): State => ({ ...state, profile })),
  on(setUserData, (state, { userData }): State => ({ ...state, userData })),
  on(setLogged, (state, { logged }): State => ({ ...state, logged })),
  on(setExpires, (state, { expires }): State => ({ ...state, expires }))
);", result);    


        }

        [Test]
        public void CreateInitalState_ReturnInitialState()
        {

            string result = CreateInitialState(@"  token: null,
  validToken: false,
  profile: null,
  userData: '',
  logged: false,
  expires: new Date()");

            Assert.AreEqual(@"// initial state
export const initialState: State = {
  token: null,
  validToken: false,
  profile: null,
  userData: '',
  logged: false,
  expires: new Date()
};", result);

        }
        [Test]
        public void CreateSelector_ReturnSelector()
        {
            string selector = CreateSelectors("Auth");

            Assert.AreEqual("export const selectAuth = createFeatureSelector<State>('auth');", selector);
        }

        private static string GetCurrentDirectory()
        {
            return GetDirectoryName(GetExecutingAssembly().Location)!;
        }
        public static EitherObject<T> GetEitherObject<T>(Either<Exception, T> either) where T : new()
        {
            Exception exception = new();
            T result = new();


            either.Match(
                Left: l => exception = l,
                Right: r => result = r);

            return new EitherObject<T>(exception, result, either.IsLeft);
        }
    }
}
