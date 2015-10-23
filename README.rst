========
Overview
========
Semiodesk.TinyVirtuoso is a tool to create small and self sustained instances of the OpenLink Virtuoso database.
This package is intended to lower the entry barrier for developers to try out Semantic Web technologies.
The goals are:
- Easy setup, just add a nuget package
- Simple first steps, a few short lines of code should be enough to get database up and running
- Easy to use, with a focus on `Semiodesk Trinity`_, makes the development of Semantic Web applications faster

The software is supported by `Semiodesk`_.
If you have any questions, suggestions or just want to tell us in which projects you are using the library, don't hesitate to `contact us`_.

Features
========
- Full fledged RDF graph database in a tiny package
- No installation required
- Easy to configure


License
=======
The TinyVirtuoso library in this repository is released under the terms of the `MIT license`_. 
This means you can use it for every project you like, even commercial ones, as long as you keep the copyright header intact. 
The source code, documentation and issue tracking can be found at our `bitbucket page`_. 
If you like what we are doing and want to support us, please consider donating.

The `OpenLink Virtuoso`_ database is released under the terms of the GPL (see TinyVirtuoso/Virtuoso/doc/LICENSE).
TinyVirtuoso does not link against OpenLink Virtuoso in any way. It just provides a way to start, stop and configure the software.
To download the source code of this software, check out the git repository at https://github.com/openlink/virtuoso-opensource/

Dependencies
============
Semiodesk.TinyVirtuoso has dependencies to 

* `OpenLink Virtuoso`_

The libraries are included in the release package. If you install via NuGet the depencies should be resolved for you.

Installation
============
The easiest way to start using the Trinity API is to add it to your project trough NuGet.

  PM> Install-Package TinyVirtuoso

Getting Started
===============
After the installation our `First Steps`_ guide should help you getting started.


Support
=======
If you need help, contact us under `hello@semiodesk.com`_.



.. GENERAL LINKS

.. _`bitbucket page`: https://bitbucket.org/semiodesk/tinyvirtuoso
.. _`Semiodesk Trinity`: http://www.semiodesk.com/products/trinity/
.. _`triplestores`: http://en.wikipedia.org/wiki/Triplestore
.. _`MIT license`: http://en.wikipedia.org/wiki/MIT_License
.. _`Semiodesk`: http://www.semiodesk.com
.. _`OpenLink Virtuoso`: https://github.com/openlink/virtuoso-opensource
.. _`First Steps`: https://bitbucket.org/semiodesk/tinyvirtuoso/wiki/FirstSteps
.. _`contact us`:mailto:hello@semiodesk.com
.. _`hello@semiodesk.com`:mailto:hello@semiodesk.com