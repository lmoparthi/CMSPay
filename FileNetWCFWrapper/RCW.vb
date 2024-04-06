Imports System.Runtime.InteropServices

' a resource class that implements the IDisposable interface
' and the IDisposable.Dispose method.
' A class that implements IDisposable.
' By implementing IDisposable, you are announcing that
' instances of this type allocate scarce resources.
Friend Class PersistentFileNetX
    Implements IDisposable
    ' An object containing COM object
    Private FNLibrary As IDMObjects.Library

    ' Track whether Dispose has been called.
    Private disposed As Boolean = False
    ' Implement IDisposable.
    ' Do not make this method virtual.
    ' A derived class should not be able to override this method.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' This object will be cleaned up by the Dispose method.
        ' Therefore, you should call GC.SupressFinalize to
        ' take this object off the finalization queue
        ' and prevent finalization code for this object
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub
    ' Dispose(bool disposing) executes in two distinct scenarios.
    ' If disposing equals true, the method has been called directly
    ' or indirectly by a user's code. Managed and unmanaged resources
    ' can be disposed.
    ' If disposing equals false, the method has been called by the
    ' runtime from inside the finalizer and you should not reference
    ' other objects. Only unmanaged resources can be disposed.
    Private Overloads Sub Dispose(ByVal disposing As Boolean)
        ' Check to see if Dispose has already been called.
        If Not Me.disposed Then
            ' If disposing equals true, dispose all managed
            ' and unmanaged resources. 
        End If

        ' Call the appropriate methods to clean up
        ' unmanaged resources here.
        ' If disposing is false,
        ' only the following code is executed.

        If (FNLibrary IsNot Nothing) Then
            Marshal.ReleaseComObject(FNLibrary)
            FNLibrary = Nothing
        End If
        'Uncomment the code line below if the current class does not implements and extends
        'a class containing dispose method.
        'MyBase.Dispose (disposing)
        disposed = True
    End Sub

    ' This finalizer will run only if the Dispose method
    ' does not get called.
    ' It gives your base class the opportunity to finalize.
    ' Do not provide finalize methods in types derived from this class.
    Protected Overrides Sub Finalize()
        ' Do not re-create Dispose clean-up code here.
        ' Calling Dispose(false) is optimal in terms of
        ' readability and maintainability.
        Dispose(False)
        MyBase.Finalize()
    End Sub
End Class