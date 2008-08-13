class AClassWithPrivateMethods:
  private def APrivateMethod():
    pass
  
  private def AnotherPrivateMethod():
    pass

class MyClass:
  def MyMethod():
    instance = AClassWithPrivateMethods()
    instance.~