class ATestClass:
  def TestMethod() as string:
    return "a string"

class ClassInTest:
  def MethodInTest():
    tc = ATestClass()
    tc.TestMethod().~