using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Unity.ImplicitCastAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ImplicitCastAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "ICA0001";

    private static readonly LocalizableString Title = "Dangerous implicit cast detected";
    private static readonly LocalizableString MessageFormat = "Implicit cast from {0} to {1} detected";
    private static readonly LocalizableString Description = "Implicit casts can lead to unexpected behavior. Consider making the cast explicit.";
    private const string Category = "Syntax";
    private const string Vector2TypeName = "UnityEngine.Vector2";
    private const string Vector3TypeName = "UnityEngine.Vector3";

    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        true,
        Description
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(AnalyzeOperation, OperationKind.Conversion);
    }

    private static void AnalyzeOperation(OperationAnalysisContext context)
    {
        var cast = (IConversionOperation) context.Operation;
        
        var typeFrom = cast?.Operand?.Type;
        var typeTo = cast?.Type;
        if (typeFrom == null || typeTo == null)
            return;

        var isV2ToV3 = typeFrom.ToDisplayString() == Vector2TypeName && typeTo.ToDisplayString() == Vector3TypeName;
        var isV3ToV2 = typeFrom.ToDisplayString() == Vector3TypeName && typeTo.ToDisplayString() == Vector2TypeName;
        if (!isV2ToV3 && !isV3ToV2)
            return;
        
        var diagnostic = Diagnostic.Create(Rule, cast!.Syntax.GetLocation(), typeFrom, typeTo);
        context.ReportDiagnostic(diagnostic);
    }
}
