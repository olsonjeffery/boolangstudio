class AClassWithProtectedMethods:
  private def AProtectedMethod():
    pass
  
  private def AnotherProtectedMethod():
    pass

class MyClass:
  def MyMethod():
    instance = AClassWithProtectedMethods()
    instance.~