# DataModelGenerator

This is an executable project that generates both the UDMF data model and its parser.  I decided I would rather rely on code generation than some kind of crazy reflection system at runtime.

Having hard-coded relative paths to the source files isn't great but this is honestly less clunky than the T4 template system I was using earlier.

## Data Model

Features of the generated UDMF data model:

* Two ways of creating:
  * Constructor - optional fields in the spec have defaults
  * Object initializers - every optional property has a default set
* A deep-clone method
* A serialization method that verifies that required properties have been set

## Parser

Parsing happens in three steps:

1) A lexer generates tokens
2) A parser generates an abstract syntax tree
3) The semantic analyzer (which is mostly generated) turns the AST into the generated model
