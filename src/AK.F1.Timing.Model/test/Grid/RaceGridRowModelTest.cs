// Copyright 2010 Andy Kernahan
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using AK.F1.Timing.Messages.Driver;

namespace AK.F1.Timing.Model.Grid
{
    public class RaceGridRowModelTest : GridRowModelTestBase<RaceGridRowModel>
    {
        protected override RaceGridRowModel CreateRow(int id)
        {
            return new RaceGridRowModel(id);
        }

        protected override IEnumerable<GridColumn> GetColumns()
        {
            yield return GridColumn.Gap;
            yield return GridColumn.Interval;
            yield return GridColumn.LapTime;
            yield return GridColumn.PitCount;
            yield return GridColumn.PitLap1;
            yield return GridColumn.PitLap2;
            yield return GridColumn.PitLap3;
        }

        protected override IEnumerable<GridColumn> GetResettableColumns()
        {
            return GetColumns();
        }
    }
}