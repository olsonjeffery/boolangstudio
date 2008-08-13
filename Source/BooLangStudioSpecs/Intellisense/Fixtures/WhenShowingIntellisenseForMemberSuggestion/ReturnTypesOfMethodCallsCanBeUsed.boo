class ATestClass:
  def TestMethod():
    pass
  
  def AnotherTestMethod():
    pass

class ClassInTest:
  def CallMe() as ATestClass:
    return ATestClass()
  
  def MethodInTest():
    CallMe().~