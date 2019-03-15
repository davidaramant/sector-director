/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.4.0.0
 */
using System.Collections.Generic;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing
{
	/// <summary>
	/// Represents a parser
	/// </summary>
	public class UdmfParser : LRkParser
	{
		/// <summary>
		/// The automaton for this parser
		/// </summary>
		private static readonly LRkAutomaton commonAutomaton = LRkAutomaton.Find(typeof(UdmfParser), "UdmfParser.bin");
		/// <summary>
		/// Contains the constant IDs for the variables and virtuals in this parser
		/// </summary>
		public class ID
		{
			/// <summary>
			/// The unique identifier for variable value
			/// </summary>
			public const int VariableValue = 0x000A;
			/// <summary>
			/// The unique identifier for variable assignment_expr
			/// </summary>
			public const int VariableAssignmentExpr = 0x000B;
			/// <summary>
			/// The unique identifier for variable block
			/// </summary>
			public const int VariableBlock = 0x000C;
			/// <summary>
			/// The unique identifier for variable global_expr
			/// </summary>
			public const int VariableGlobalExpr = 0x000D;
			/// <summary>
			/// The unique identifier for variable translation_unit
			/// </summary>
			public const int VariableTranslationUnit = 0x000E;
		}
		/// <summary>
		/// The collection of variables matched by this parser
		/// </summary>
		/// <remarks>
		/// The variables are in an order consistent with the automaton,
		/// so that variable indices in the automaton can be used to retrieve the variables in this table
		/// </remarks>
		private static readonly Symbol[] variables = {
			new Symbol(0x000A, "value"), 
			new Symbol(0x000B, "assignment_expr"), 
			new Symbol(0x000C, "block"), 
			new Symbol(0x000D, "global_expr"), 
			new Symbol(0x000E, "translation_unit"), 
			new Symbol(0x0012, "__V18"), 
			new Symbol(0x0014, "__V20"), 
			new Symbol(0x0015, "__VAxiom") };
		/// <summary>
		/// The collection of virtuals matched by this parser
		/// </summary>
		/// <remarks>
		/// The virtuals are in an order consistent with the automaton,
		/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table
		/// </remarks>
		private static readonly Symbol[] virtuals = {
 };
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		public UdmfParser(UdmfLexer lexer) : base (commonAutomaton, variables, virtuals, null, lexer) { }

		/// <summary>
		/// Visitor interface
		/// </summary>
		public class Visitor
		{
			public virtual void OnTerminalWhiteSpace(ASTNode node) {}
			public virtual void OnTerminalSeparator(ASTNode node) {}
			public virtual void OnTerminalKeyword(ASTNode node) {}
			public virtual void OnTerminalQuotedString(ASTNode node) {}
			public virtual void OnTerminalInteger(ASTNode node) {}
			public virtual void OnTerminalFloat(ASTNode node) {}
			public virtual void OnTerminalIdentifier(ASTNode node) {}
			public virtual void OnVariableValue(ASTNode node) {}
			public virtual void OnVariableAssignmentExpr(ASTNode node) {}
			public virtual void OnVariableBlock(ASTNode node) {}
			public virtual void OnVariableGlobalExpr(ASTNode node) {}
			public virtual void OnVariableTranslationUnit(ASTNode node) {}
		}

		/// <summary>
		/// Walk the AST using a visitor
		/// </summary>
		public static void Visit(ParseResult result, Visitor visitor)
		{
			VisitASTNode(result.Root, visitor);
		}

		/// <summary>
		/// Walk the AST using a visitor
		/// </summary>
		public static void VisitASTNode(ASTNode node, Visitor visitor)
		{
			for (int i = 0; i < node.Children.Count; i++)
				VisitASTNode(node.Children[i], visitor);
			switch(node.Symbol.ID)
			{
				case 0x0003: visitor.OnTerminalWhiteSpace(node); break;
				case 0x0004: visitor.OnTerminalSeparator(node); break;
				case 0x0005: visitor.OnTerminalKeyword(node); break;
				case 0x0006: visitor.OnTerminalQuotedString(node); break;
				case 0x0007: visitor.OnTerminalInteger(node); break;
				case 0x0008: visitor.OnTerminalFloat(node); break;
				case 0x0009: visitor.OnTerminalIdentifier(node); break;
				case 0x000A: visitor.OnVariableValue(node); break;
				case 0x000B: visitor.OnVariableAssignmentExpr(node); break;
				case 0x000C: visitor.OnVariableBlock(node); break;
				case 0x000D: visitor.OnVariableGlobalExpr(node); break;
				case 0x000E: visitor.OnVariableTranslationUnit(node); break;
			}
		}
	}
}
