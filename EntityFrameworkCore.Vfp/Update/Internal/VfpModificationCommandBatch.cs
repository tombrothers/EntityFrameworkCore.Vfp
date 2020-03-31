using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Update.Internal {
    public class VfpModificationCommandBatch : AffectedCountModificationCommandBatch {
        private const int MaxScriptLength = 8000;
        private const int MaxParameterCount = 2100;
        private const int MaxRowCount = 1000;
        private int _parameterCount = 1; // Implicit parameter for the command text
        private readonly int _maxBatchSize;
        private int _commandsLeftToLengthCheck = 50;

        public VfpModificationCommandBatch(
            [NotNull] ModificationCommandBatchFactoryDependencies dependencies,
            int? maxBatchSize
        ) : base(dependencies) {
            if(maxBatchSize.HasValue && maxBatchSize.Value <= 0) {
                throw new ArgumentOutOfRangeException(nameof(maxBatchSize), RelationalStrings.InvalidMaxBatchSize);
            }

            _maxBatchSize = Math.Min(maxBatchSize ?? int.MaxValue, MaxRowCount);
        }

        protected override bool CanAddCommand([NotNull] ModificationCommand modificationCommand) {
            if(ModificationCommands.Count >= _maxBatchSize) {
                return false;
            }

            var additionalParameterCount = CountParameters(modificationCommand);

            if(_parameterCount + additionalParameterCount >= MaxParameterCount) {
                return false;
            }

            _parameterCount += additionalParameterCount;
            return true;
        }

        protected override bool IsCommandTextValid() {
            if(--_commandsLeftToLengthCheck < 0) {
                var commandTextLength = GetCommandText().Length;
                if(commandTextLength >= MaxScriptLength) {
                    return false;
                }

                var averageCommandLength = commandTextLength / ModificationCommands.Count;
                var expectedAdditionalCommandCapacity = (MaxScriptLength - commandTextLength) / averageCommandLength;
                _commandsLeftToLengthCheck = Math.Max(1, expectedAdditionalCommandCapacity / 4);
            }

            return true;
        }

        protected override int GetParameterCount() => _parameterCount;

        private static int CountParameters(ModificationCommand modificationCommand) {
            var parameterCount = 0;

            for(var columnIndex = 0; columnIndex < modificationCommand.ColumnModifications.Count; columnIndex++) {
                var columnModification = modificationCommand.ColumnModifications[columnIndex];

                if(columnModification.UseCurrentValueParameter) {
                    parameterCount++;
                }

                if(columnModification.UseOriginalValueParameter) {
                    parameterCount++;
                }
            }

            return parameterCount;
        }
    }
}
