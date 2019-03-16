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

Parsing happens in two steps:

1) The [Hime](https://cenotelie.fr/projects/hime/) parser generator framework handles parsing a stream to an abstract syntax tree (that's the `.gram` file).  The generator program assumes that Hime is "installed" in a folder named `hime` at the base of the repo (this is the contents of the zip download from the site).
2) A custom generated semantic analyzer that maps the Hime AST to the data model.
