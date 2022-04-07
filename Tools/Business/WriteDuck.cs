using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tools.Business
{
    public static class WriteDuck
    {

        public static string SetUpImports(string stateName)
        {
            string result = $@"import {{
  createAction,
  props,
  createReducer,
  on,
  createFeatureSelector
}} from '@ngrx/store';
import {{ {stateName} as State }} from './{stateName}.model';

// actions";
            return result;
        }

        public static string CreateAction(string key, string value, string duckName)
        {
            string action = @$"export const set{UpperFirstLetter(key)} = createAction(
  '[{duckName.ToUpper()}] SET_{key.ToUpper()}',
  props<{{ {key}: {value} }}>()
);
";
            return action;
        }

        public static string CreateReducer(string name,  ImmutableList<string> properties)
        {
            return $@"// reducer
export const {name.ToLower()}Reducer = createReducer(
  initialState,
 {string.Join(",\r\n ",properties.Select(x => $" on(set{UpperFirstLetter(x)}, (state, {{ {x} }}): State => ({{ ...state, {x} }}))").ToList())}
);";
        }

        private static string UpperFirstLetter(string str)
        {
            if (str.Length == 0)
                return "Empty String";
            else if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str[1..];
        }

        public static string CreateInitialState( string propertiesFromState)
        {
            return $@"// initial state
export const initialState: State = {{
{propertiesFromState.Replace(";", ",")}
}};";

        }

        public static string CreateSelectors( string selectorName)
        {
            return $"export const select{selectorName} = createFeatureSelector<State>('{selectorName.ToLower()}');";
        }


    }
}
