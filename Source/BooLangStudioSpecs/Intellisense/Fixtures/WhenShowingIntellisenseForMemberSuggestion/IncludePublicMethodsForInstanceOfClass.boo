class TheClassToReference:
  def FirstMethod():
    pass
  
  def SecondMethod():
    pass

class MyClass:
  def MyMethod():
    instance = TheClassToReference()
    instance.~