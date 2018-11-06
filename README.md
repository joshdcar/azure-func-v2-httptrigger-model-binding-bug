# azure-func-v2-httptrigger-model-binding-bug
---

Using the HttpTrigger bindings in Azure Functions 2.0 to a POCO class is exhibiting problems when binding to a property that is of some sort of collection type (including array, Ienumerable, etc). 
Properties of these types are always NULL.  Manual serialization using JsonConvert is successful. 
---
In Azure Function 1.x (see PocoSerializerTestAF1 project) the model binding works as expected.
---
In Azure Functions 2.x (See PocoSerializerTest project) the POCO property that is a collection does not get serialized and is always null.
---

