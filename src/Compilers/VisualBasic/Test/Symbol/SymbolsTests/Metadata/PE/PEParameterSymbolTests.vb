﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Reflection
Imports Xunit

Namespace Microsoft.CodeAnalysis.VisualBasic.UnitTests

    Public Class PEParameterSymbolTests
        Inherits BasicTestBase

        <Fact()>
        Public Sub NoParameterNames()
            ' Create simple interface where method parameters have no names.
            ' Interface I
            '   Sub M(... As Object, ... As Object)
            ' End Interface
            Dim reference = CreateReflectionEmitAssembly(
                Sub(moduleBuilder)
                    Dim typeBuilder = moduleBuilder.DefineType(
                        "I",
                        TypeAttributes.Interface Or TypeAttributes.Public Or TypeAttributes.Abstract)
                    Dim methodBuilder = typeBuilder.DefineMethod(
                        "M",
                        MethodAttributes.Public Or MethodAttributes.Abstract Or MethodAttributes.Virtual,
                        GetType(Void),
                        {GetType(Object), GetType(Object)})
                    methodBuilder.DefineParameter(1, ParameterAttributes.None, Nothing)
                    methodBuilder.DefineParameter(2, ParameterAttributes.None, Nothing)
                    typeBuilder.CreateType()
                End Sub)
            Dim source =
                <compilation>
                    <file name="c.vb"><![CDATA[
Module M
    Sub M(o As I)
        o.M(1, Param:=2)
    End Sub
End Module
]]>
                    </file>
                </compilation>
            Dim compilation = CreateCompilationWithMscorlibAndVBRuntimeAndReferences(source, {reference})
            compilation.AssertTheseDiagnostics(
<expected>
BC30455: Argument not specified for parameter 'Param' of 'Sub M(Param As Object, Param As Object)'.
        o.M(1, Param:=2)
          ~
BC30274: Parameter 'Param' of 'Sub M(Param As Object, Param As Object)' already has a matching argument.
        o.M(1, Param:=2)
               ~~~~~
</expected>)
        End Sub

    End Class
End Namespace

