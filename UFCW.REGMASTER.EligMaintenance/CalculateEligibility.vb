Option Explicit On
Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.Data.Common

<Serializable()> _
Public Class CalculateEligibility

#Region "Variables"
    '' 'Private Shared _TraceSwitch As New BooleanSwitch("EligTraceCloning", "Trace Switch in App.Config")

    Private _familyID As Integer
    Private _eligPeriod As Date
    Private _WeightedHours As DataTable

    Private _DataDS As New DataSet

    Private _EligCalcElements As New DataTable
    '' Private _mthdtlcount As New DataTable
    '' Private _dteligperiod As New DataTable
    ''Private _term As New DataTable
    ''Private _nhw As New DataTable
    ''Private _cobrapayment As New DataTable
    ''Private _negativerows As New DataTable
    ''Private _weightedhrslookup As New DataTable
    ''Private _cobraactive As New DataTable
    ''Private _memtype As New DataTable
    ''Private _calcA2cnt As New DataTable
    ''Private _meddenplan As New DataTable
    ''Private _retplan As New DataTable
    ''Private _retromemtype As New DataTable
    ''Private _cobraqe As New DataTable
    ''Private _retireeacctno As New DataTable

    ''OPEN  TERMCURSOR;
    ''OPEN  NHWCURSOR;
    ''OPEN  COBRAPAYMENTCURSOR;
    ''OPEN  NEGATIVEROWSCURSOR;
    ''OPEN WTHRLUCURSOR;
    ''OPEN COBRAACTIVECURSOR;
    ''OPEN MEMTYPECURSOR;
    ''OPEN CALA2COUNTCURSOR;
    ''OPEN MEDDENPLANSCURSOR;
    ''OPEN RETPLANCURSOR;
    ''OPEN RETROMEMTYPECURSOR;
    ''OPEN COBRAQECURSOR;

    Private _drweightedHours As DataRow
    Private _dreligcalcElements As DataRow


    ''Private vMTH_STATUS As String
    ''Private vMTH_PLANTYPE As String
    ''Private vMTH_MEMTYPE As String
    ''Private vMTH_LOCALNO As Integer
    ''Private vMTH_MED_PLAN As Integer
    ''Private vMTH_DEN_PLAN As Integer
    ''Private vMTH_MED_ELG_SW As Integer
    ''Private vMTH_DEN_ELG_SW As Integer
    ''Private vMTH_PREMIUM_SW As Integer
    ''Private vMTH_FAMILY_SW As Integer
    ''Private vMTH_RET_PLAN As String
    ''Private vMTH_A2COUNT As Integer
    ''Private vDUAL_COVERAGE_SW As Integer
    ''Private vSTOP_HRA_FUNDING_SW As Integer
    ''Private vSTOP_SPOUSE_HRQ_IND As Integer
    ''Private vHRA_PRIME_SW As Integer
    ''Private vPROCESS_COPAY_SW As Integer
    ''Private vMTHPLAN_AB_1ST As Date
    ''Private vMTH_BREAK_IN_SERVICE_SW As Integer

    ''Private vENROLLED_IN_COBRA As Integer = 0
    ''Private vNOT_ENROLLED_IN_COBRA As Integer = 0
    ''Private vCOBRA_IS_PAID As Integer = 0

    ''Private vSTATUS_IS_UNCHANGED As Integer = 0
    '' ''Private vSTATUS_HAS_CHANGED As Integer = 0

    ''Private vELGACCTHRHOURS_CODE As String = ""
    ''Private vWTHRLU_COBRA_PRIME_IND As Integer = 0
    ''Private vWTHRLU_GROUP_LEVEL As String = ""
    ''Private vWTHRLU_MED_ELIG_SW As Integer = 0
    ''Private vWTHRLU_DEN_ELIG_SW As Integer = 0
    ''Private vWTHRLU_STATUS As String = ""

    ''Private vHRS_WAIT_PER_STATUS_TEXT As String = ""

    ''Private vMEMPLAN_GROUP_LEVEL As String = ""
    ''Private vWS_LOOKUP_MEMTYPE As String = ""
    ''Private vWS_XREF_MEMTYPE As String = ""
    ''Private vRETURNED_ROW_COUNT As Int16 = 0

#End Region

#Region "Constructors"

    Private Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal familyid As Integer, ByVal eligPeriod As Date)

        MyBase.New()
        _familyID = familyid
        _eligPeriod = eligPeriod

    End Sub

#End Region

#Region "Functions"

    Public Function RetrieveRowStatusbeforeEligcalculation(ByVal familyid As Integer, ByVal eligPeriod As Date) As Boolean
        Try
            '' Retrieve  statuses on rows before elig Calculation
            Dim RowStatusDS As DataSet = RegMasterDAL.RetrieveRowStatusbeforeEligcalculation(_familyID, _eligPeriod)

            If RowStatusDS IsNot Nothing AndAlso RowStatusDS.Tables.Count > 0 Then

                If CInt(RowStatusDS.Tables(0).Rows(0)(0)) > 0 Then
                    Return True
                Else
                    MessageBox.Show(" There are no changes for Eligibility to calculate.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Else
                MessageBox.Show("There are no changes for Eligibility to calculate.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                Return False
            End If
        End Try

    End Function

    ''Public Function determineEligibility(ByVal familyid As Integer, ByVal eligPeriod As Date) As Boolean
    ''    Dim status As Boolean = False
    ''    Dim Transaction As DbTransaction = Nothing

    ''    Try
    ''        Try

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("MM/dd/yyyy HH:mm:ss.fff") & " Calculating Eligibility for Family " & CStr(familyid) & "  for the Eligibility Period " & CStr(eligPeriod), CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Environment.NewLine, "CalcEligibility.txt")
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Retrieving data from ELIGCALELEMENTSCURSOR and MTHDTLCOUNTCURSOR", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            _data = RegMasterDAL.RetrieveCommonDataforEligcalculation(_familyID, _eligPeriod)   '' Retrieve require common data from other tables



    ''            If _data IsNot Nothing AndAlso _data.Tables.Count > 0 Then
    ''                _eligcalcElements = _data.Tables(0)
    ''                _mthdtlcount = _data.Tables(1)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Got data from ELIGCALELEMENTSCURSOR and MTHDTLCOUNTCURSOR", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            Else
    ''                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before calculation Eligibility.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " No data from ELIGCALELEMENTSCURSOR and MTHDTLCOUNTCURSOR", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                Return False
    ''            End If

    ''            '' No row from elig cal elements

    ''            If (_eligcalcElements IsNot Nothing) AndAlso (_eligcalcElements.Rows.Count > 0) Then
    ''                _dreligcalcElements = _eligcalcElements.Rows(0)
    ''            Else
    ''                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before calculation Eligibility.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " No data from ELIGCALELEMENTSCURSOR ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                Return False
    ''            End If

    ''            '' to be calculated elig period is Not between  threshhold and eligperiod range from query


    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Retrieving data from WEIGHTEDHOURSCURSOR", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            _weightedHours = RegMasterDAL.GetWeightedhoursandStatus(_familyID, _eligPeriod, CStr(_dreligcalcElements("PLANTYPE")), CStr(_dreligcalcElements("STATUS")), CStr(_dreligcalcElements("LAST_MEMTYPE")))   '' from weighted hours query

    ''            If (_weightedHours IsNot Nothing) AndAlso (_weightedHours.Rows.Count > 0) Then

    ''                _drweightedHours = _weightedHours.Rows(0)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Got data from WEIGHTEDHOURSCURSOR", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            Else
    ''                MessageBox.Show("Eligibility is not updatable." & Environment.NewLine & " Eligibility period is too old.", "Old Period", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " No data from WEIGHTEDHOURSCURSOR ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                Return False
    ''            End If

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Retrieving data from 12 cursors", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            _data = RegMasterDAL.RetrieveDataforEligcalculation(_familyID, _eligPeriod)   '' Retrieve require data from other tables


    ''            _term = _data.Tables(0)
    ''            _nhw = _data.Tables(1)
    ''            _cobrapayment = _data.Tables(2)
    ''            _negativerows = _data.Tables(3)
    ''            _weightedhrslookup = _data.Tables(4)
    ''            _cobraactive = _data.Tables(5)
    ''            _memtype = _data.Tables(6)
    ''            _calcA2cnt = _data.Tables(7)
    ''            _meddenplan = _data.Tables(8)
    ''            _retplan = _data.Tables(9)
    ''            _retromemtype = _data.Tables(10)
    ''            _cobraqe = _data.Tables(11)
    ''            _retireeacctno = _data.Tables(12)

    ''        Catch ex As Exception

    ''            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''            If (rethrow) Then
    ''                Throw
    ''            Else
    ''                Return False
    ''            End If
    ''        End Try

    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Begin the transaction", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''        Transaction = RegMasterDAL.BeginTransaction

    ''        If SaveChanges(Transaction) = True Then
    ''            RegMasterDAL.CommitTransaction(Transaction)
    ''            status = True
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Commit the transaction", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''        Else
    ''            RegMasterDAL.RollbackTransaction(Transaction)
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Rollback the transaction", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            status = False
    ''        End If

    ''    Catch ex As Exception
    ''        RegMasterDAL.RollbackTransaction(Transaction)
    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Rollback the transaction", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        Else
    ''            status = False
    ''        End If
    ''    Finally
    ''        If Transaction IsNot Nothing Then
    ''            Transaction.Dispose()
    ''            Transaction = Nothing
    ''        End If
    ''        disposeobjects()
    ''    End Try
    ''    Return status
    ''End Function

    ''Private Function getcurrentvalues(ByRef Transaction As DbTransaction) As Boolean

    ''    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & "I am in getcurrentvalues method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''    Try
    ''        ''  --- START HERE
    ''        ''  --- THIS IS FOR A BRAND NEW ROW IN MONTHLY DETAIL.
    ''        ''  --- BECAUSE IT IS FOR THE CURRENT PERIOD, IT USES THE
    ''        ''  --- LAST MEMTYPE, STATUS, PLANTYPE, FAMILY_ELIG_MO
    ''        ''  --- AND LOCAL FROM THE ELGCALC ELEMENTS / MEMPLAN FETCH.
    ''        ''  --- MEDICAL PLANS, A2COUNT AND PLAN AB 1ST ELGDATE ARE
    ''        ''  --- ACQUIRED IN SEPARATE CALLED PARAGRAPHS.
    ''        ''   -- INSERT CURRENT ROW  --- PERFORM 6000-INSERT-MTHDTL-CURRENT   THRU 6000-EXIT


    ''        '' -- IF WE GET NEGATIVE HOURS CHECK TERM FOR ACTIVE STATUS AND NHW FOR RETIREE STATUS. COBRA HOURS ARE ALWAYS +VE. 0 OR ANY NUMBER
    ''        If CInt(_drweightedHours("WEIGHTED_HOURS")) < 0 Then
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                If CInt(_term.Rows(1)(0)) > 0 Then
    ''                    ''N/E account no
    ''                    vMTH_STATUS = "ACTIVE"
    ''                    '' 
    ''                ElseIf CInt(_retireeacctno.Rows(0)(0)) > 0 Then
    ''                    '' Retiree account exists for that period

    ''                    vMTH_STATUS = "RETIREE"

    ''                    ''---- PERFORM RP00-GET-RET-PLAN  and other info             THRU RP00-EXIT.
    ''                    If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''                        vMTH_PLANTYPE = CStr(_retplan.Rows(0)("RETPLANTYPE"))
    ''                        vMTH_MEMTYPE = CStr(_retplan.Rows(0)("RETMEMTYPE"))
    ''                    Else
    ''                        vMTH_PLANTYPE = ""
    ''                        vMTH_MEMTYPE = ""
    ''                    End If
    ''                    ''---  THRU RP00-EXIT.

    ''                    vWTHRLU_GROUP_LEVEL = "RETIREE"                    ''to get the correct lookup hours

    ''                Else
    ''                    If CInt(_term.Rows(0)(0)) > 0 Then
    ''                        MessageBox.Show("TERMED STATUS FOUND AND NO ROW INSERTED IN  MTHDTL" & Environment.NewLine & " ELIGIBILITY WILL NOT BE CALCULATED", "TERMED STATUS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " TERMED STATUS FOUND AND NO ROW INSERTED IN  MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return True
    ''                    End If
    ''                End If
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "RETIREE" Then
    ''                If CInt(_nhw.Rows(0)(0)) = 0 Then
    ''                    MessageBox.Show("NHW not found  FOUND AND NO ROW INSERTED IN  MTHDTL" & Environment.NewLine & " ELIGIBILITY WILL NOT BE CALCULATED", "No NHW", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NHW not found  FOUND AND NO ROW INSERTED IN  MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    Return True
    ''                End If
    ''            End If
    ''        End If

    ''        ''--- 4010-CHECK-FOR-COBRA.
    ''        vENROLLED_IN_COBRA = 1 ''true

    ''        ''--- LOOK FOR A ROW IN THE COBRA ENROLLED TABLE, GET THE
    ''        ''--- MOST RECENT OCCURANCE IF THERE IS ONE. IF NONE FOUND
    ''        ''--- SET THE SWITCH TO NOT-ENROLLED-IN-COBRA FOR LATER TESTING
    ''        If (_cobraqe IsNot Nothing) AndAlso (_cobraqe.Rows.Count > 0) Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " row found  in the COBRA enrolled table", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vENROLLED_IN_COBRA = 1
    ''        Else
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " no row found  in the COBRA enrolled table", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vENROLLED_IN_COBRA = 0
    ''            vNOT_ENROLLED_IN_COBRA = 1
    ''        End If
    ''        ''----4010-EXIT. EXIT.


    ''        ''--- SOMTIMES WE HAVE A PERSON WHO IS TERMED BEFORE THEY
    ''        ''--- EVER COMPLETE THEIR WAITING PERIOD. THERFORE WE WANT
    ''        ''--- TO CHECK THIS FIRST BEFORE WE GO THROUGH THE PROCESS
    ''        ''--- OF DETERMINING WAITING STATUS AND ELIGIBILITY.
    ''        ''--- THE CHECK-FOR-TERM ONLY LOOKS FOR ACCOUNT 9986
    ''        ''--- WHEN THE SUM OF THE WEIGHTED HOURS IS -1000.


    ''        vMTH_BREAK_IN_SERVICE_SW = 0
    ''        If (vMTH_STATUS) Is Nothing Then vMTH_STATUS = CStr(_dreligcalcElements("STATUS"))
    ''        If (vMTH_PLANTYPE) Is Nothing Then vMTH_PLANTYPE = CStr(_dreligcalcElements("PLANTYPE"))
    ''        If (vMTH_MEMTYPE) Is Nothing Then vMTH_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''        vMTH_LOCALNO = CInt(_dreligcalcElements("LAST_LOCAL"))

    ''        '' vSTATUS_IS_UNCHANGED = 1  '' true

    ''        If CStr(_drweightedHours("WAIT_PER_STATUS")).ToUpper = "INCOMPLETE" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is INCOMPLETE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTH_MED_ELG_SW = 0
    ''            vMTH_DEN_ELG_SW = 0
    ''            '' -----	GO TO 6000-CONTINUE
    ''        Else
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is COMPLETE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            ''----- PERFORM TW00-TEST-WEIGHTED-HOURS  THRU TW00-EXIT
    ''            '' --- THIS PARAGRAPH ACCESSES THE WEIGHTED HOURS LOOKUP TABLE TO
    ''            '' --- DETERMINE IF THE STATUS OF THE PARTICIPANT IS ACTIVE,
    ''            '' --- RETIREE OR COBRA AND IF THE ASSOCIATED HOURS GRANT THE
    ''            '' --- PARTICIPANT ELIGIBILITY BASED UPON THAT STATUS.
    ''            '' --- EARLIER IN THE PROGRAM WE JOINED TO FDBWRK.COBRA_ENROLLED
    ''            '' --- TO DETERMINE IF AN OPEN ROW EXISTS WITH OUR PROCESSING
    ''            '' --- ELIGIBILITY PERIOD BETWEEN THE LOST COVERAGE DATE AND
    ''            '' --- AND THE FINAL COBRA DATE (ESSENTIALLY FROM AND THROUGH
    ''            '' --- DATES OF THE COBRA ENROLLMENT ROW.)
    ''            '' --- SINCE THE PRESENCE OF AN OPEN COBRA ROW INDICATES THAT
    ''            '' --- THE PARTICIPANT IS ENROLLED IN COBRA, WE ALSO NEED TO
    ''            '' --- VERIFY THAT THE PAYMENT HAS BEEN MADE. THEREFORE
    ''            '' --- WE NEED TO CONTROL THE PROCESSING BASED UPON 2 THINGS:
    ''            '' --- FIRST - IS THE PARTICIPANT ACTIVELY ENROLLED IN COBRA
    ''            '' --- SECOND - IF ENROLLED, HAS THE PAYMENT BEEN MADE AT THE
    ''            '' --- TIME OF POSTING.
    ''            '' --- IF NOT ENROLLED, WE SET THE PRIME INDICATOR TO 9
    ''            '' --- IF ENROLLED AND NOT PAID, WE ALSO SET THE INDICATOR TO 9
    ''            '' --- SO THAT THE WEIGHTED HOURS THAT ARE PRESENT ARE
    ''            '' --- CORRECTLY EVALUATED AS SUCH.
    ''            '' --- 2000-FETCH-ELGCURS WHICH JOINS TO OUR FDBWRK.COBRA_ENROLLED
    ''            '' --- TABLE USING FROM/THRU DATES, FAMILY_ID AND ROW_STATUS = 'O'.
    ''            '' --- UNTIL COBRA IS REDESIGNED TO WORK WITH PREMIUMS, THIS VALUE
    ''            '' --- 0 = ENROLLED BUT NOT PRIME IS NOT CONSIDERED.
    ''            '' --- ONLY THESE ARE VALIDE.
    ''            '' --- 9 = NOT ENROLLED IN COBRA OR ENROLLED AND NOT PAID.
    ''            '' --- 1 = ENROLLED AND PRIME

    ''            '' --- TW00-TEST-WEIGHTED-HOURS.



    ''            If vENROLLED_IN_COBRA = 1 Then
    ''                vCOBRA_IS_PAID = 1  ''true
    ''                ''--- WE CHECK TO SEE IF THERE IS A ROW IN ELIG_ACCT_HOURS.
    ''                ''--- IF THERE IS ONE, THEN WE KNOW THE PAYMENT HAS BEEN
    ''                ''--- MADE. COBRA IS PRIME AT THIS WRITING, THEREFORE
    ''                ''--- WHEN A ROW IS FOUND, WE KNOW WE HAVE A COBRA PAYMENT
    ''                ''--- AND THEREFOR A COBRA STATUS IN MONTHLY DETAIL.
    ''                ''--- TWCB00-CHECK-COBRA-PAYMENT
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF THERE IS A COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                If (_cobrapayment IsNot Nothing) AndAlso (_cobrapayment.Rows.Count > 0) Then

    ''                    '' -- CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS
    ''                    ''-- AND SET THE COBRA  PRIME INDICATOR TO 2 IF WE
    ''                    ''-- FIND MORE THAN 1 ROW.
    ''                    ''-- NG00-COUNT-NEGATIVE-ROWS
    ''                    vELGACCTHRHOURS_CODE = CStr(_cobrapayment.Rows(0)(0))

    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS TO SET THE COBRA  PRIME INDICATOR TO 2", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                    If (_negativerows IsNot Nothing) AndAlso (_negativerows.Rows.Count > 0) Then
    ''                        If CInt(_negativerows.Rows(0)(0)) > 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " MULTIPLE NEGATIVE ROWS FOUND", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            vWTHRLU_COBRA_PRIME_IND = 2
    ''                        End If
    ''                        vCOBRA_IS_PAID = 1
    ''                    End If
    ''                Else
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NO COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    vCOBRA_IS_PAID = 0
    ''                End If                             '' end of _cobrapayment


    ''                If vWTHRLU_COBRA_PRIME_IND = 2 Then
    ''                    vCOBRA_IS_PAID = 1
    ''                Else
    ''                    If vCOBRA_IS_PAID = 1 Then
    ''                        vWTHRLU_COBRA_PRIME_IND = 2
    ''                    Else
    ''                        vWTHRLU_COBRA_PRIME_IND = 9
    ''                    End If
    ''                End If
    ''            Else
    ''                vWTHRLU_COBRA_PRIME_IND = 9
    ''            End If                 '' end of  vENROLLED_IN_COBRA


    ''            '' SET GROUP LEVEL
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " SET THE GROUP LEVEL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            If _eligPeriod < RegMasterDAL._GlobalEligPeriod Then
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_drweightedHours("MTH_GROUP_LEVEL"))
    ''            Else
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_dreligcalcElements("GROUP_LEVEL"))
    ''            End If


    ''            Dim strfilter As String = CInt(_drweightedHours("WEIGHTED_HOURS")) & " >=  MIN_HOURS AND " & CInt(_drweightedHours("WEIGHTED_HOURS")) & " <= MAX_HOURS AND GROUP_LEVEL = '" & vWTHRLU_GROUP_LEVEL & "' AND COBRA_PRIME_IND = " & CStr(vWTHRLU_COBRA_PRIME_IND)
    ''            Dim DR As DataRow() = _weightedhrslookup.Select(strfilter)

    ''            If DR.Count > 0 Then
    ''                vWTHRLU_MED_ELIG_SW = CInt(DR(0)("MED_ELIG_SW"))       '' from   WEIGHTED_HOURS_LOOKUP table
    ''                vWTHRLU_DEN_ELIG_SW = CInt(DR(0)("DEN_ELIG_SW"))
    ''                vWTHRLU_STATUS = CStr(DR(0)("STATUS"))

    ''                If (vENROLLED_IN_COBRA = 1) AndAlso (vELGACCTHRHOURS_CODE = "B") Then
    ''                    vWTHRLU_DEN_ELIG_SW = 0
    ''                End If
    ''            Else
    ''                vWTHRLU_MED_ELIG_SW = 0
    ''                vWTHRLU_DEN_ELIG_SW = 0
    ''                vWTHRLU_STATUS = "UNKNOWN"
    ''                MessageBox.Show("FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE" & Environment.NewLine & "PLEASE NOTIFY IS DEPARTMENT AS IT IS CRITICAL TO CALCULATE ELIGIBILITY ", "WEIGHTED_HOURS_LOOKUP", MessageBoxButtons.OK, MessageBoxIcon.Error)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            End If


    ''            ''---TW00-EXIT

    ''            If CStr(DR(0)("STATUS")) <> vMTH_STATUS Then
    ''                vSTATUS_IS_UNCHANGED = 1
    ''            End If

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ASSIGNING MED_ELIG_SW, DEN_ELIG_SW VALUES", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            vMTH_MED_ELG_SW = vWTHRLU_MED_ELIG_SW       '' from   WEIGHTED_HOURS_LOOKUP table
    ''            vMTH_DEN_ELG_SW = vWTHRLU_DEN_ELIG_SW
    ''            vMTH_STATUS = vWTHRLU_STATUS
    ''        End If                 '' End of Incomplete


    ''        ''IF STATUS-IS-UNCHANGED
    ''        ''   GO TO 6000-CONTINUE
    ''        '' END-IF.

    ''        If vSTATUS_IS_UNCHANGED = 1 Then


    ''            ''--- ELGMTHDTL-STATUS IS SET IN TW00- PARAGRAPH.
    ''            ''--- WHEN WE GET HERE WE MUST DETERMINE WHAT KIND OF COBRA
    ''            ''--- AND CHANGE THE MEMTYPE ON THE ROW TO REFLECT IT'S
    ''            ''--- COBRA EQUIVALENT.  THIS IS NECESSARY FOR TRANSITIONAL
    ''            ''--- PROCESSING. ONCE CONVERTED, THE LAST-MEMTYPE ON
    ''            ''--- ELGCALC ELEMENTS WILL ALREADY BE SET TO COBRA
    ''            ''------  EVALUATE ELGMTHDTL-STATUS-TEXT

    ''            If vMTH_STATUS.ToUpper = "COBRA" Then
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ELGMTHDTL-STATUS-TEXT -- STAUS COBRA ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                ''     ---CB00-GET-A-OR-B-COBRA-DATA   THRU CB00-EXIT
    ''                ''--- IT IS NOT ENOUGH TO JUST BE ENROLLED IN COBRA TO
    ''                ''--- BE ELIGIBLE. THE PAYMENT MUST HAVE ALSO BEEN RECEIVED
    ''                ''--- AND POSTED. WHEN THE WEIGHTED HOURS IS NEGATIVE
    ''                ''---	CB00-GET-A-OR-B-COBRA-DATA.

    ''                If (_cobraactive IsNot Nothing) AndAlso (_cobraactive.Rows.Count > 0) Then


    ''                    ''  --- UPDATE THE DENTAL SWITCH IF THE PLAN CODE IS FOUND  AND IS AN 'A'.
    ''                    If CStr(_cobraactive.Rows(0)("HOURS_CODE")) = "A" Then
    ''                        vMTH_MED_ELG_SW = 1
    ''                        vMTH_DEN_ELG_SW = 1

    ''                    ElseIf CStr(_cobraactive.Rows(0)("HOURS_CODE")) = "B" Then

    ''                        vMTH_MED_ELG_SW = 1
    ''                        vMTH_DEN_ELG_SW = 0
    ''                    End If
    ''                Else ''--------------NO ROWS IN  _cobraactive
    ''                    vMTH_MED_ELG_SW = 0
    ''                    vMTH_DEN_ELG_SW = 0
    ''                End If
    ''                ''  ----- THRU CB00-EXIT


    ''                ''--- MOVE ELGCALC-LAST-MEMTYPE TO WS-LOOKUP-MEMTYPE
    ''                vWS_LOOKUP_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))

    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " GET MEMTYPE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                ''---- PERFORM MX00-GET-COBRA-MEMTYPE-XREF   THRU MX00-EXIT

    ''                '' --- IF THE MEMTYPE WE ARE LOOKING UP IN THIS PARAGRAPH
    ''                '' --- IS ALREADY A COBRA MEMTYPE, THEN WE WILL NOT GET A
    ''                '' --- HIT AND WE WILL MAINTAIN THE EXISTING MEMTYPE.
    ''                '' --- WS-LOOKUP-MEMTYPE IS THE EXISTING MEMTYPE IN ELGCALC
    ''                '' --- ELEMENTS. THIS IS ONLY EXECUTED IF THE STATUS IS COBRA.

    ''                If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then


    ''                    Dim DR As DataRow() = _memtype.Select(" MEMTYPE = '" & vWS_LOOKUP_MEMTYPE & "' AND STATUS IN ('ACTIVE','RETIREE') ")
    ''                    If DR.Count > 0 Then

    ''                        ''---MX10-UPDATE-ELGCALC-MEMTYPE  THRU MX10-EXIT

    ''                        vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateEligCalelementsfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN ELGCALC ELEMENTS ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            ''updated
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "ELGCALC ELEMENTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            '' Return False
    ''                        End If
    ''                        ''	---- THRU MX10-EXIT



    ''                        ''---EVALUATE WS-XREF-PLANTYPE
    ''                        If CStr(_memtype.Rows(0)("ELIG_RANKING")).ToUpper = "PLAN110" Then
    ''                            '' ----	    PERFORM MX20-UPDATE-A2COUNT-MEMTYPE      THRU MX20-EXIT
    ''                            vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                            If vRETURNED_ROW_COUNT = 1 Then
    ''                                ''updated
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Else
    ''                                MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN A2BKCNT " & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATE MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                                '' Return False
    ''                            End If
    ''                        End If
    ''                        ''---MX20-EXIT.

    ''                    Else
    ''                        '' no rows in _memtype select
    ''                        ''-- MOVE WS-LOOKUP-MEMTYPE TO WS-XREF-MEMTYPE
    ''                        vWS_XREF_MEMTYPE = vWS_LOOKUP_MEMTYPE
    ''                    End If
    ''                    ''  THRU MX00-EXIT
    ''                    vMTH_MEMTYPE = vWS_XREF_MEMTYPE
    ''                End If



    ''            ElseIf vMTH_STATUS.ToUpper = "ACTIVE" Then
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ELGMTHDTL-STATUS-TEXT -- STAUS ACTIVE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                ''--- MOVE ELGCALC-LAST-MEMTYPE TO WS-LOOKUP-MEMTYPE
    ''                vWS_LOOKUP_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''                ''--- PERFORM MX01-GET-ACTIVE-MEMTYPE-XREF  THRU MX01-EXIT

    ''                ''					 --- IF THE MEMTYPE WE ARE LOOKING UP IN THIS PARAGRAPH
    ''                ''					 --- IS ALREADY AN ACTIVE MEMTYPE, THEN WE WILL NOT GET A
    ''                ''					 --- HIT AND WE WILL MAINTAIN THE EXISTING MEMTYPE.
    ''                ''					 --- WS-LOOKUP-MEMTYPE IS THE EXISTING MEMTYPE IN ELGCALC
    ''                ''					 --- ELEMENTS. THIS IS ONLY EXECUTED IF THE STATUS IS ACTIVE.
    ''                ''					 --- AND ONLY IF THERE HAS BEEN A CHANGE IN STATUS.


    ''                If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''                    Dim DR As DataRow() = _memtype.Select(" MEMTYPE ='" & vWS_LOOKUP_MEMTYPE & "' AND STATUS IN ('COBRA','RETIREE') ")
    ''                    If DR.Count > 0 Then

    ''                        ''---MX10-UPDATE-ELGCALC-MEMTYPE  THRU MX10-EXIT

    ''                        vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateEligCalelementsfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            ''updated
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS " & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "ELGCALC ELEMENTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return False
    ''                        End If
    ''                        ''	---- THRU MX10-EXIT



    ''                        ''---EVALUATE WS-XREF-PLANTYPE
    ''                        If CStr(_memtype.Rows(0)("ELIG_RANKING")).ToUpper = "PLAN110" Then
    ''                            '' ----	    PERFORM MX20-UPDATE-A2COUNT-MEMTYPE      THRU MX20-EXIT
    ''                            vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                            If vRETURNED_ROW_COUNT = 1 Then
    ''                                ''updated
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Else
    ''                                MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                                Return False
    ''                            End If
    ''                        End If
    ''                        ''---MX20-EXIT.

    ''                    Else
    ''                        '' no rows in _memtype select
    ''                        ''-- MOVE WS-LOOKUP-MEMTYPE TO WS-XREF-MEMTYPE
    ''                        vWS_XREF_MEMTYPE = vWS_LOOKUP_MEMTYPE
    ''                    End If

    ''                    '' THRU MX01-EXIT
    ''                    vMTH_MEMTYPE = vWS_XREF_MEMTYPE
    ''                End If

    ''            End If  ''IF vMTH_STATUS = 'COBRA' THEN STATEMENT
    ''            ''------   END-EVALUATE  ELGMTHDTL-STATUS-TEXT
    ''        End If            ''end if of If vSTATUS_IS_UNCHANGED = 1 Then



    ''        ''--- DETERMINE WHAT TO PUT IN PLAN-AB-1ST-ELGDATE AND
    ''        ''	--- A2COUNT FOR ACTIVES. SET ALL OTHERS TO DEFAULT
    ''        ''--- 6000-CONTINUE.
    ''        ''---- EVALUATE MEMPLAN-ELIG-RANKING-TEXT

    ''        If CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "PLAN110" OrElse CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "COBRA110" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---PLAN110 ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")

    ''            ''---  PERFORM A200-GET-A2COUNT          THRU A200-EXIT
    ''            ''   -- FOR PROCESSING OF HOURS THAT ARE RECEIVED AFTER THE
    ''            ''   -- A2BKCNT-OLDESTHRS DATE WE DO THE FOLLOWING:
    ''            ''   -- WHEN DETERMINING THE A2COUNT FOR A ROW, FIRST CHECK TO SEE IF
    ''            ''   -- THE PARTICIPANT IS BROKEN. IF SO, THEN THE COUNT WILL BE SET
    ''            ''   -- TO 3 AND THE OLDEST HOURS SET TO THE WORK PERIOD BEING
    ''            ''   -- PROCESSED.
    ''            ''   -- WHEN PROCESSING ROWS THAT PRECEEDE THE A2BKCNT-OLDESTHRS DATE
    ''            ''   -- WE SET THE A2COUNT IN MONTHLY DETAIL TO ZERO AND EXIT.


    ''            If (_calcA2cnt IsNot Nothing) AndAlso (_calcA2cnt.Rows.Count = 0) Then

    ''                vMTH_A2COUNT = 0
    ''            Else
    ''                ''   -- IF WE GOT A SUCCESSFUL RETURN CODE, WE NEED TO
    ''                ''-- DETERMINE IF WE ARE PROCESSING A PERIOD PRIOR TO
    ''                ''-- OUR OLDEST HOURS. IF THE OLDEST HOURS IS THE
    ''                ''-- DEFAULT OF 9999-12-31 OR PRECEEDES OUR OLDESTHRS
    ''                ''-- THEN WE CAN'T ACCURATELY DETERMINE THE A2COUNT
    ''                ''-- SO WE GO TO THE EXIT. A2BKCNT-OLDESTHRS WAS
    ''                ''-- ACQUIRED IN PARAGRAPH 1000-DECLARE-OPEN-ELGCURS.
    ''                If CDate(_dreligcalcElements("OLDESTHRS")) = CDate("9999-12-31") OrElse _eligPeriod < CDate(_dreligcalcElements("OLDESTHRS")) Then
    ''                    vMTH_A2COUNT = 0
    ''                End If

    ''            End If

    ''            ''------ EVALUATE MEMPLAN-STATUS-TEXT
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                If CInt(_calcA2cnt.Rows(0)("A2COUNT")) = 0 Then
    ''                    ''------ PERFORM A210-RESET-PLAN110     THRU A210-EXIT
    ''                    ''			-- THIS WILL UPDATE THE A2COUNT AND OLDEST HOURS FOR A
    ''                    ''			-- PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE

    ''                    vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, _eligPeriod, Transaction))

    ''                    If vRETURNED_ROW_COUNT = 1 Then
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED A2COUNT IN A2BKCNT .. PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        ''updated
    ''                    Else
    ''                        MessageBox.Show("FAILURE TO UPDATE A2COUNT IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE A2COUNT IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return False
    ''                    End If
    ''                End If

    ''                ''----THRU A210-EXIT
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("MONTHCOUNT"))
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "COBRA" Then
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("A2COUNT"))

    ''            Else
    ''                vMTH_A2COUNT = 0
    ''            End If
    ''            ''------ END-EVALUATE.

    ''            ''  ---- THRU A200-EXIT
    ''        ElseIf CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "CURRENT" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---CURRENT ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTHPLAN_AB_1ST = CDate(_drweightedHours("PLAN_AB_1ST_ELGDATE"))
    ''        Else
    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")
    ''            vMTH_A2COUNT = 0
    ''        End If
    ''        ''--- END-EVALUATE MEMPLAN-ELIG-RANKING-TEXT

    ''        ''---- PERFORM MD00-GET-MED-AND-DEN-PLANS      THRU MD00-EXIT.
    ''        If (_meddenplan IsNot Nothing) AndAlso (_meddenplan.Rows.Count > 0) Then
    ''            vMTH_MED_PLAN = CInt(_meddenplan.Rows(0)("MED_COVERAGE"))
    ''            vMTH_DEN_PLAN = CInt(_meddenplan.Rows(0)("DENT_COVERAGE"))

    ''        Else
    ''            vMTH_MED_PLAN = 0
    ''            vMTH_DEN_PLAN = 0

    ''        End If
    ''        ''------THRU MD00-EXIT.

    ''        ''----- PERFORM CP00-CHECK-PREMIUM-SW          THRU CP00-EXIT.
    ''        If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''            Dim DR As DataRow() = _memtype.Select(" MEMTYPE ='" & vMTH_MEMTYPE & "'")
    ''            If DR.Count > 0 Then

    ''                If CStr(DR(0)("PREMREQ")).ToUpper = "N" Then
    ''                    vMTH_PREMIUM_SW = 0
    ''                ElseIf CStr(DR(0)("PREMREQ")).ToUpper = "Y" Then
    ''                    vMTH_PREMIUM_SW = 1
    ''                Else
    ''                    vMTH_PREMIUM_SW = 1
    ''                End If

    ''            Else
    ''                vMTH_PREMIUM_SW = 1
    ''            End If
    ''        End If
    ''        ''----  THRU CP00-EXIT.

    ''        ''---- PERFORM RP00-GET-RET-PLAN               THRU RP00-EXIT.
    ''        If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''            vMTH_RET_PLAN = CStr(_retplan.Rows(0)("RETPLAN"))
    ''        Else
    ''            vMTH_RET_PLAN = ""
    ''        End If
    ''        ''---  THRU RP00-EXIT.

    ''        '' ----- FAMILY_SW ------

    ''        If CInt(_dreligcalcElements("FAM_ELIG_MO")) < 999 Then
    ''            vMTH_FAMILY_SW = 1
    ''        Else
    ''            vMTH_FAMILY_SW = 0
    ''        End If

    ''        ''------ PERFORM IM00-INSERT-MONTHLY-DETAIL   

    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " completed getcurrentvalues method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''    Catch ex As Exception
    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''            Return False
    ''        End If
    ''    Finally
    ''    End Try
    ''    Return True
    ''End Function

    ''Private Function getpriorvalues(ByRef Transaction As DbTransaction) As Boolean
    ''    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " I am in getpriorvalues method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''    Try
    ''        ''--- WE OFTEN GET STAND ALONE RETRO TERMINATIONS. THERE IS
    ''        ''--- NO REASON TO ENTER A ROW IN MONTHLY DETAIL FOR THEM,
    ''        ''--- THEREFORE A MESSAGE WILL BE DISPLAYED ON THE SYSOUT
    ''        ''--- FOR BOTH ACTIVE AND RETIREES.
    ''        ''--- IF HOWEVER THERE IS A NHW ROW FOR A RETIREE, WE WILL
    ''        ''--- INSERT A ROW IN MONTHLY DETAIL.
    ''        ''--- WHEN WE DO INSERT A RETRO ROW, WE REPORT ON IT AS
    ''        ''--- WELL AT THE END OF THIS PARAGRAPH, JUST BEFORE THE EXIT.

    ''        ''--  6500-INSERT-MTHDTL-PRIOR.


    ''        '' -- IF WE GET NEGATIVE HOURS CHECK TERM FOR ACTIVE STATUS AND NHW FOR RETIREE STATUS.
    ''        If CInt(_drweightedHours("WEIGHTED_HOURS")) < 0 Then
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                ''  check for any N/E account  or retiree row existing for that period
    ''                If CInt(_term.Rows(1)(0)) > 0 Then
    ''                    ''N/E account no
    ''                    vMTH_STATUS = "ACTIVE"
    ''                    '' 
    ''                ElseIf CInt(_retireeacctno.Rows(0)(0)) > 0 Then
    ''                    '' Retiree account exists for that period

    ''                    vMTH_STATUS = "RETIREE"

    ''                    ''---- PERFORM RP00-GET-RET-PLAN  and other info             THRU RP00-EXIT.
    ''                    If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''                        vMTH_PLANTYPE = CStr(_retplan.Rows(0)("RETPLANTYPE"))
    ''                        vMTH_MEMTYPE = CStr(_retplan.Rows(0)("RETMEMTYPE"))
    ''                    Else
    ''                        vMTH_PLANTYPE = ""
    ''                        vMTH_MEMTYPE = ""
    ''                    End If
    ''                    ''---  THRU RP00-EXIT.

    ''                    vWTHRLU_GROUP_LEVEL = "RETIREE"                    ''to get the correct lookup hours

    ''                Else
    ''                    If CInt(_term.Rows(0)(0)) > 0 Then
    ''                        MessageBox.Show(" TERMED STATUS FOUND AND NO ROW INSERTED IN  MTHDTL " & Environment.NewLine & " ELIGIBILITY WILL NOT BE CALCULATED", "TERMED STATUS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " TERMED STATUS FOUND AND NO ROW INSERTED IN  MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return True
    ''                    End If
    ''                End If
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "RETIREE" Then
    ''                If CInt(_nhw.Rows(0)(0)) = 0 Then
    ''                    MessageBox.Show(" NHW not found  FOUND AND NO ROW INSERTED IN  MTHDTL" & Environment.NewLine & " ELIGIBILITY WILL NOT BE CALCULATED", "No NHW", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NHW not found  FOUND AND NO ROW INSERTED IN  MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    Return True
    ''                End If
    ''            End If
    ''        End If


    ''        vMTH_BREAK_IN_SERVICE_SW = 0

    ''        ''--- GET THE MEMTYPE OFF THE ACCT HOURS ROW
    ''        ''--- IF THIS PARAGRAPH FINDS A ROW, IT REPLACES
    ''        ''--- ELGCALC-LAST-MEMTYPE, MEMPLAN-STATUS, AND
    ''        ''--- MEMPLAN-PLANTYPE WITH WHAT IT FINDS, OTHERWISE
    ''        ''--- IT USES WHAT IT FOUND FROM THE ORIGINAL QUERY IN
    ''        ''--- PARAGRAPH 1000-DECLARE-OPEN-ELGCURS.

    ''        ''---  PERFORM 6560-GET-RETRO-MEMTYPE   THRU 6560-EXIT.
    ''        If vMTH_STATUS IsNot Nothing Then
    ''            ''set in values in the top
    ''        Else


    ''            If (_retromemtype IsNot Nothing) AndAlso (_retromemtype.Rows.Count > 0) Then

    ''                vMTH_STATUS = CStr(_retromemtype.Rows(0)("STATUS"))
    ''                vMTH_PLANTYPE = CStr(_retromemtype.Rows(0)("PLANTYPE"))
    ''                vMTH_MEMTYPE = CStr(_retromemtype.Rows(0)("MEMTYPE"))
    ''                vMTH_LOCALNO = CInt(CStr(_dreligcalcElements("LAST_LOCAL")))
    ''            Else
    ''                '' most of the time this loop will get executed as we are entering special hours.
    ''                '' no row in elig_acct_hours for special accounts we are defaluting values from elig_calc_elments.
    ''                vMTH_STATUS = CStr(_dreligcalcElements("STATUS"))
    ''                vMTH_PLANTYPE = CStr(_dreligcalcElements("PLANTYPE"))
    ''                vMTH_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''                vMTH_LOCALNO = CInt(_dreligcalcElements("LAST_LOCAL"))
    ''            End If
    ''        End If
    ''        ''---  THRU 6560-EXIT.

    ''        vHRS_WAIT_PER_STATUS_TEXT = CStr(_drweightedHours("WAIT_PER_STATUS")).ToUpper

    ''        ''         -- ALLOW FOR A RETRO COBRA PAYMENT


    ''        If ((_retromemtype IsNot Nothing) AndAlso (_retromemtype.Rows.Count > 0)) AndAlso CStr(_retromemtype.Rows(0)("STATUS")) = "COBRA" Then
    ''            vHRS_WAIT_PER_STATUS_TEXT = "COMPLETE"
    ''        End If

    ''        If vHRS_WAIT_PER_STATUS_TEXT = "INCOMPLETE" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is INCOMPLETE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTH_MED_ELG_SW = 0
    ''            vMTH_DEN_ELG_SW = 0
    ''        Else
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is COMPLETE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            ''----- PERFORM TW00-TEST-WEIGHTED-HOURS  THRU TW00-EXIT
    ''            '' --- THIS PARAGRAPH ACCESSES THE WEIGHTED HOURS LOOKUP TABLE TO
    ''            '' --- DETERMINE IF THE STATUS OF THE PARTICIPANT IS ACTIVE,
    ''            '' --- RETIREE OR COBRA AND IF THE ASSOCIATED HOURS GRANT THE
    ''            '' --- PARTICIPANT ELIGIBILITY BASED UPON THAT STATUS.
    ''            '' --- EARLIER IN THE PROGRAM WE JOINED TO FDBWRK.COBRA_ENROLLED
    ''            '' --- TO DETERMINE IF AN OPEN ROW EXISTS WITH OUR PROCESSING
    ''            '' --- ELIGIBILITY PERIOD BETWEEN THE LOST COVERAGE DATE AND
    ''            '' --- AND THE FINAL COBRA DATE (ESSENTIALLY FROM AND THROUGH
    ''            '' --- DATES OF THE COBRA ENROLLMENT ROW.)
    ''            '' --- SINCE THE PRESENCE OF AN OPEN COBRA ROW INDICATES THAT
    ''            '' --- THE PARTICIPANT IS ENROLLED IN COBRA, WE ALSO NEED TO
    ''            '' --- VERIFY THAT THE PAYMENT HAS BEEN MADE. THEREFORE
    ''            '' --- WE NEED TO CONTROL THE PROCESSING BASED UPON 2 THINGS:
    ''            '' --- FIRST - IS THE PARTICIPANT ACTIVELY ENROLLED IN COBRA
    ''            '' --- SECOND - IF ENROLLED, HAS THE PAYMENT BEEN MADE AT THE
    ''            '' --- TIME OF POSTING.
    ''            '' --- IF NOT ENROLLED, WE SET THE PRIME INDICATOR TO 9
    ''            '' --- IF ENROLLED AND NOT PAID, WE ALSO SET THE INDICATOR TO 9
    ''            '' --- SO THAT THE WEIGHTED HOURS THAT ARE PRESENT ARE
    ''            '' --- CORRECTLY EVALUATED AS SUCH.
    ''            '' --- 2000-FETCH-ELGCURS WHICH JOINS TO OUR FDBWRK.COBRA_ENROLLED
    ''            '' --- TABLE USING FROM/THRU DATES, FAMILY_ID AND ROW_STATUS = 'O'.
    ''            '' --- UNTIL COBRA IS REDESIGNED TO WORK WITH PREMIUMS, THIS VALUE
    ''            '' --- 0 = ENROLLED BUT NOT PRIME IS NOT CONSIDERED.
    ''            '' --- ONLY THESE ARE VALIDE.
    ''            '' --- 9 = NOT ENROLLED IN COBRA OR ENROLLED AND NOT PAID.
    ''            '' --- 1 = ENROLLED AND PRIME

    ''            '' --- TW00-TEST-WEIGHTED-HOURS.



    ''            If vENROLLED_IN_COBRA = 1 Then
    ''                vCOBRA_IS_PAID = 1  ''true
    ''                ''--- WE CHECK TO SEE IF THERE IS A ROW IN ELIG_ACCT_HOURS.
    ''                ''--- IF THERE IS ONE, THEN WE KNOW THE PAYMENT HAS BEEN
    ''                ''--- MADE. COBRA IS PRIME AT THIS WRITING, THEREFORE
    ''                ''--- WHEN A ROW IS FOUND, WE KNOW WE HAVE A COBRA PAYMENT
    ''                ''--- AND THEREFOR A COBRA STATUS IN MONTHLY DETAIL.
    ''                ''--- TWCB00-CHECK-COBRA-PAYMENT
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF THERE IS A COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                If (_cobrapayment IsNot Nothing) AndAlso (_cobrapayment.Rows.Count > 0) Then
    ''                    '' -- CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS
    ''                    ''-- AND SET THE COBRA  PRIME INDICATOR TO 2 IF WE
    ''                    ''-- FIND MORE THAN 1 ROW.
    ''                    ''-- NG00-COUNT-NEGATIVE-ROWS
    ''                    vELGACCTHRHOURS_CODE = CStr(_cobrapayment.Rows(0)(0))

    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS TO SET THE COBRA  PRIME INDICATOR TO 2", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    If (_negativerows IsNot Nothing) AndAlso (_negativerows.Rows.Count > 0) Then
    ''                        If CInt(_negativerows.Rows(0)(0)) > 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " MULTIPLE NEGATIVE ROWS FOUND", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            vWTHRLU_COBRA_PRIME_IND = 2
    ''                        End If
    ''                        vCOBRA_IS_PAID = 1
    ''                    End If
    ''                Else
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NO COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    vCOBRA_IS_PAID = 0
    ''                End If                             '' end of _cobrapayment

    ''                If vWTHRLU_COBRA_PRIME_IND = 2 Then
    ''                    vCOBRA_IS_PAID = 1
    ''                Else
    ''                    If vCOBRA_IS_PAID = 1 Then
    ''                        vWTHRLU_COBRA_PRIME_IND = 2
    ''                    Else
    ''                        vWTHRLU_COBRA_PRIME_IND = 9
    ''                    End If
    ''                End If
    ''            Else
    ''                vWTHRLU_COBRA_PRIME_IND = 9
    ''            End If                 '' end of  vENROLLED_IN_COBRA


    ''            '' SET GROUP LEVEL
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " SET THE GROUP LEVEL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            If _eligPeriod < RegMasterDAL._GlobalEligPeriod Then
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_drweightedHours("MTH_GROUP_LEVEL"))
    ''            Else
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_dreligcalcElements("GROUP_LEVEL"))
    ''            End If


    ''            Dim strfilter As String = CInt(_drweightedHours("WEIGHTED_HOURS")) & " >=  MIN_HOURS AND " & CInt(_drweightedHours("WEIGHTED_HOURS")) & " <= MAX_HOURS AND GROUP_LEVEL = '" & vWTHRLU_GROUP_LEVEL & "' AND COBRA_PRIME_IND = " & CStr(vWTHRLU_COBRA_PRIME_IND)
    ''            Dim DR As DataRow() = _weightedhrslookup.Select(strfilter)

    ''            If DR.Count > 0 Then
    ''                vWTHRLU_MED_ELIG_SW = CInt(DR(0)("MED_ELIG_SW"))       '' from   WEIGHTED_HOURS_LOOKUP table
    ''                vWTHRLU_DEN_ELIG_SW = CInt(DR(0)("DEN_ELIG_SW"))
    ''                vWTHRLU_STATUS = CStr(DR(0)("STATUS"))

    ''                If (vENROLLED_IN_COBRA = 1) AndAlso (vELGACCTHRHOURS_CODE = "B") Then
    ''                    vWTHRLU_DEN_ELIG_SW = 0
    ''                End If
    ''            Else
    ''                vWTHRLU_MED_ELIG_SW = 0
    ''                vWTHRLU_DEN_ELIG_SW = 0
    ''                vWTHRLU_STATUS = "UNKNOWN"
    ''                MessageBox.Show("FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE" & Environment.NewLine & "PLEASE NOTIFY IS DEPARTMENT AS IT IS CRITICAL TO CALCULATE ELIGIBILITY ", "WEIGHTED_HOURS_LOOKUP", MessageBoxButtons.OK, MessageBoxIcon.Error)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            End If

    ''            ''---TW00-EXIT
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ASSIGNING MED_ELIG_SW, DEN_ELIG_SW VALUES", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTH_MED_ELG_SW = vWTHRLU_MED_ELIG_SW       '' from   WEIGHTED_HOURS_LOOKUP table
    ''            vMTH_DEN_ELG_SW = vWTHRLU_DEN_ELIG_SW
    ''            vMTH_STATUS = vWTHRLU_STATUS
    ''        End If                 '' End of Incomplete



    ''        If CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "PLAN110" OrElse CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "COBRA110" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---PLAN110 ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")

    ''            ''---  PERFORM A200-GET-A2COUNT          THRU A200-EXIT
    ''            ''   -- FOR PROCESSING OF HOURS THAT ARE RECEIVED AFTER THE
    ''            ''   -- A2BKCNT-OLDESTHRS DATE WE DO THE FOLLOWING:
    ''            ''   -- WHEN DETERMINING THE A2COUNT FOR A ROW, FIRST CHECK TO SEE IF
    ''            ''   -- THE PARTICIPANT IS BROKEN. IF SO, THEN THE COUNT WILL BE SET
    ''            ''   -- TO 3 AND THE OLDEST HOURS SET TO THE WORK PERIOD BEING
    ''            ''   -- PROCESSED.
    ''            ''   -- WHEN PROCESSING ROWS THAT PRECEEDE THE A2BKCNT-OLDESTHRS DATE
    ''            ''   -- WE SET THE A2COUNT IN MONTHLY DETAIL TO ZERO AND EXIT.


    ''            If (_calcA2cnt IsNot Nothing) AndAlso (_calcA2cnt.Rows.Count = 0) Then

    ''                vMTH_A2COUNT = 0
    ''            Else
    ''                ''   -- IF WE GOT A SUCCESSFUL RETURN CODE, WE NEED TO
    ''                ''-- DETERMINE IF WE ARE PROCESSING A PERIOD PRIOR TO
    ''                ''-- OUR OLDEST HOURS. IF THE OLDEST HOURS IS THE
    ''                ''-- DEFAULT OF 9999-12-31 OR PRECEEDES OUR OLDESTHRS
    ''                ''-- THEN WE CAN'T ACCURATELY DETERMINE THE A2COUNT
    ''                ''-- SO WE GO TO THE EXIT. A2BKCNT-OLDESTHRS WAS
    ''                ''-- ACQUIRED IN PARAGRAPH 1000-DECLARE-OPEN-ELGCURS.
    ''                If CDate(_dreligcalcElements("OLDESTHRS")) = CDate("9999-12-31") OrElse _eligPeriod < CDate(_dreligcalcElements("OLDESTHRS")) Then
    ''                    vMTH_A2COUNT = 0
    ''                End If

    ''            End If

    ''            ''------ EVALUATE MEMPLAN-STATUS-TEXT
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                If CInt(_calcA2cnt.Rows(0)("A2COUNT")) = 0 Then
    ''                    ''------ PERFORM A210-RESET-PLAN110     THRU A210-EXIT
    ''                    ''			-- THIS WILL UPDATE THE A2COUNT AND OLDEST HOURS FOR A
    ''                    ''			-- PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE

    ''                    vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, _eligPeriod, Transaction))

    ''                    If vRETURNED_ROW_COUNT = 1 Then
    ''                        ''updated
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED A2COUNT IN A2BKCNT .. PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    Else
    ''                        MessageBox.Show("FAILURE TO UPDATE A2COUNT IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE A2COUNT IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    End If
    ''                End If
    ''                ''----THRU A210-EXIT
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("A2COUNT"))
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "COBRA" Then
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("A2COUNT"))

    ''            Else
    ''                vMTH_A2COUNT = 0
    ''            End If
    ''            ''------ END-EVALUATE.

    ''            ''  ---- THRU A200-EXIT
    ''        ElseIf CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "CURRENT" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---CURRENT ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTHPLAN_AB_1ST = CDate(_drweightedHours("PLAN_AB_1ST_ELGDATE"))

    ''        Else
    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")
    ''            vMTH_A2COUNT = 0
    ''        End If
    ''        ''--- END-EVALUATE MEMPLAN-ELIG-RANKING-TEXT


    ''        ''---- PERFORM MD00-GET-MED-AND-DEN-PLANS      THRU MD00-EXIT.
    ''        If (_meddenplan IsNot Nothing) AndAlso (_meddenplan.Rows.Count > 0) Then
    ''            vMTH_MED_PLAN = CInt(_meddenplan.Rows(0)("MED_COVERAGE"))
    ''            vMTH_DEN_PLAN = CInt(_meddenplan.Rows(0)("DENT_COVERAGE"))

    ''        Else
    ''            vMTH_MED_PLAN = 0
    ''            vMTH_DEN_PLAN = 0

    ''        End If
    ''        ''------THRU MD00-EXIT.

    ''        ''----- PERFORM CP00-CHECK-PREMIUM-SW          THRU CP00-EXIT.
    ''        If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''            Dim DR As DataRow() = _memtype.Select(" MEMTYPE ='" & vMTH_MEMTYPE & "'")
    ''            If DR.Count > 0 Then

    ''                If CStr(DR(0)("PREMREQ")).ToUpper = "N" Then
    ''                    vMTH_PREMIUM_SW = 0
    ''                ElseIf CStr(DR(0)("PREMREQ")).ToUpper = "Y" Then
    ''                    vMTH_PREMIUM_SW = 1
    ''                Else
    ''                    vMTH_PREMIUM_SW = 1
    ''                End If

    ''            Else
    ''                vMTH_PREMIUM_SW = 1
    ''            End If
    ''        End If
    ''        ''----  THRU CP00-EXIT.

    ''        ''---- PERFORM RP00-GET-RET-PLAN               THRU RP00-EXIT.
    ''        If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''            vMTH_RET_PLAN = CStr(_retplan.Rows(0)("RETPLAN"))
    ''        Else
    ''            vMTH_RET_PLAN = ""
    ''        End If
    ''        ''---  THRU RP00-EXIT.

    ''        '' ----- FAMILY_SW ------

    ''        If CInt(_dreligcalcElements("FAM_ELIG_MO")) < 999 Then
    ''            vMTH_FAMILY_SW = 1
    ''        Else
    ''            vMTH_FAMILY_SW = 0
    ''        End If

    ''        ''------ PERFORM IM00-INSERT-MONTHLY-DETAIL   
    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & "completed getpriorvalues method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''        Return True

    ''        '' 6500-EXIT.  EXIT.


    ''    Catch ex As Exception
    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        End If
    ''        Return False
    ''    Finally

    ''    End Try
    ''End Function

    ''Private Function updatablevalues(ByRef Transaction As DbTransaction) As Boolean
    ''    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " I am in updatevalues method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''    Try

    ''        '' -- IF WE GET NEGATIVE HOURS CHECK TERM FOR ACTIVE STATUS AND NHW FOR RETIREE STATUS. COBRA HOURS ARE ALWAYS +VE. 0 OR ANY NUMBER
    ''        If CInt(_drweightedHours("WEIGHTED_HOURS")) < 0 Then
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                If CInt(_term.Rows(1)(0)) > 0 Then
    ''                    ''N/E account no
    ''                    vMTH_STATUS = "ACTIVE"
    ''                    '' 
    ''                ElseIf CInt(_retireeacctno.Rows(0)(0)) > 0 Then
    ''                    '' Retiree account exists for that period

    ''                    vMTH_STATUS = "RETIREE"

    ''                    ''---- PERFORM RP00-GET-RET-PLAN  and other info             THRU RP00-EXIT.
    ''                    If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''                        vMTH_PLANTYPE = CStr(_retplan.Rows(0)("RETPLANTYPE"))
    ''                        vMTH_MEMTYPE = CStr(_retplan.Rows(0)("RETMEMTYPE"))
    ''                    Else
    ''                        vMTH_PLANTYPE = ""
    ''                        vMTH_MEMTYPE = ""
    ''                    End If
    ''                    ''---  THRU RP00-EXIT.

    ''                    vWTHRLU_GROUP_LEVEL = "RETIREE"                    ''to get the correct lookup hours
    ''                Else
    ''                    If CInt(_term.Rows(0)(0)) > 0 Then
    ''                        MessageBox.Show("TERMED STATUS FOUND AND ROW WAS DELETED FROM MTHDTL", "TERMED STATUS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        ''---       PERFORM 7100-DELETE-TERMED-STATUS-ROW   THRU 7100-EXIT
    ''                        '' -- THIS PARAGRAPH GETS EXECUTED WHEN WE ARE UPDATING AN
    ''                        ''-- EXISTING ROW WITH NEGATIVE WEIGHTED HOURS AND NO NHW
    ''                        ''-- RETIREE ROW IS THERE TO PREVENT THE DELETION.
    ''                        vRETURNED_ROW_COUNT = CShort(RegMasterDAL.DeleteMTHDTLfromEligCalculation(_familyID, _eligPeriod, Transaction))
    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            ''deleted
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " TERMED STATUS FOUND AND ROW WAS DELETED FROM MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return True '' for no rows inserted
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO DELETE ROW FROM MTHDTL", "MTHDTL", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO DELETE ROW FROM MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Eligibility calculation exited", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return False
    ''                        End If

    ''                    Else    '' NO ROWS FROM TERM



    ''                    End If
    ''                End If
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "RETIREE" Then
    ''                If CInt(_nhw.Rows(0)(0)) = 0 Then
    ''                    MessageBox.Show("NHW NOT FOUND AND ROW WAS DELETED FROM MTHDTL", "MTHDTL", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                    ''---       PERFORM 7100-DELETE-TERMED-STATUS-ROW   THRU 7100-EXIT
    ''                    vRETURNED_ROW_COUNT = CShort(RegMasterDAL.DeleteMTHDTLfromEligCalculation(_familyID, _eligPeriod, Transaction))

    ''                    If vRETURNED_ROW_COUNT = 1 Then
    ''                        ''deleted
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NHW NOT FOUND AND ROW WAS DELETED FROM MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return True
    ''                    Else
    ''                        MessageBox.Show("FAILURE TO DELETE ROW FROM MTHDTL", "MTHDTL", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO DELETE ROW FROM MTHDTL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return False
    ''                    End If

    ''                Else

    ''                End If
    ''            End If    ''  "ACTIVE" 
    ''        End If            ''<0


    ''        If (vMTH_STATUS) Is Nothing Then vMTH_STATUS = CStr(_dreligcalcElements("STATUS"))
    ''        If (vMTH_PLANTYPE) Is Nothing Then vMTH_PLANTYPE = CStr(_dreligcalcElements("PLANTYPE"))
    ''        If (vMTH_MEMTYPE) Is Nothing Then vMTH_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''        vMTH_LOCALNO = CInt(_dreligcalcElements("LAST_LOCAL"))

    ''        If CStr(_drweightedHours("WAIT_PER_STATUS")).ToUpper = "INCOMPLETE" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is INCOMPLETE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTH_MED_ELG_SW = 0
    ''            vMTH_DEN_ELG_SW = 0
    ''            '' -----	GO TO 7000-CONTINUE
    ''        Else

    ''            '' --- TW00-TEST-WEIGHTED-HOURS.
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " wait per status is COMPLETE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")


    ''            If vENROLLED_IN_COBRA = 1 Then
    ''                vCOBRA_IS_PAID = 1  ''true
    ''                ''--- WE CHECK TO SEE IF THERE IS A ROW IN ELIG_ACCT_HOURS.
    ''                ''--- IF THERE IS ONE, THEN WE KNOW THE PAYMENT HAS BEEN
    ''                ''--- MADE. COBRA IS PRIME AT THIS WRITING, THEREFORE
    ''                ''--- WHEN A ROW IS FOUND, WE KNOW WE HAVE A COBRA PAYMENT
    ''                ''--- AND THEREFOR A COBRA STATUS IN MONTHLY DETAIL.
    ''                ''--- TWCB00-CHECK-COBRA-PAYMENT
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF THERE IS A COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                If (_cobrapayment IsNot Nothing) AndAlso (_cobrapayment.Rows.Count > 0) Then
    ''                    '' -- CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS
    ''                    ''-- AND SET THE COBRA  PRIME INDICATOR TO 2 IF WE
    ''                    ''-- FIND MORE THAN 1 ROW.
    ''                    ''-- NG00-COUNT-NEGATIVE-ROWS

    ''                    vELGACCTHRHOURS_CODE = CStr(_cobrapayment.Rows(0)(0))

    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " CHECK TO SEE IF WE  HAVE MULTIPLE NEGATIVE ROWS TO SET THE COBRA  PRIME INDICATOR TO 2", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                    If (_negativerows IsNot Nothing) AndAlso (_negativerows.Rows.Count > 0) Then
    ''                        If CInt(_negativerows.Rows(0)(0)) > 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " MULTIPLE NEGATIVE ROWS FOUND", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            vWTHRLU_COBRA_PRIME_IND = 2
    ''                        End If
    ''                        vCOBRA_IS_PAID = 1
    ''                    End If
    ''                Else
    ''                    'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " NO COBRA ROW IN ELIG_ACCT_HOURS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    vCOBRA_IS_PAID = 0
    ''                End If                             '' end of _cobrapayment

    ''                If vWTHRLU_COBRA_PRIME_IND = 2 Then
    ''                    vCOBRA_IS_PAID = 1
    ''                Else
    ''                    If vCOBRA_IS_PAID = 1 Then
    ''                        vWTHRLU_COBRA_PRIME_IND = 2
    ''                    Else
    ''                        vWTHRLU_COBRA_PRIME_IND = 9
    ''                    End If
    ''                End If
    ''            Else
    ''                vWTHRLU_COBRA_PRIME_IND = 9
    ''            End If                 '' end of  vENROLLED_IN_COBRA


    ''            '' SET GROUP LEVEL
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " SET THE GROUP LEVEL", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            If _eligPeriod < RegMasterDAL._GlobalEligPeriod Then
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_drweightedHours("MTH_GROUP_LEVEL"))
    ''            Else
    ''                If vWTHRLU_GROUP_LEVEL = "" Then vWTHRLU_GROUP_LEVEL = CStr(_dreligcalcElements("GROUP_LEVEL"))
    ''            End If

    ''            Dim strfilter As String = CInt(_drweightedHours("WEIGHTED_HOURS")) & " >=  MIN_HOURS AND " & CInt(_drweightedHours("WEIGHTED_HOURS")) & " <= MAX_HOURS AND GROUP_LEVEL = '" & vWTHRLU_GROUP_LEVEL & "' AND COBRA_PRIME_IND = " & CStr(vWTHRLU_COBRA_PRIME_IND)
    ''            Dim DR As DataRow() = _weightedhrslookup.Select(strfilter)

    ''            If DR.Count > 0 Then
    ''                vWTHRLU_MED_ELIG_SW = CInt(DR(0)("MED_ELIG_SW"))       '' from   WEIGHTED_HOURS_LOOKUP table
    ''                vWTHRLU_DEN_ELIG_SW = CInt(DR(0)("DEN_ELIG_SW"))


    ''                '' THIS IS THE CODE TO UPDATE MTH_DTL, ELIG_CALC_ELEMENTS  MEMTYPE AUTOMATICALLY WHEN STATUS CHNAGES
    ''                '' WE ONLY CONSIDERS ACTIVE - RETIREE
    ''                If vMTH_STATUS.ToUpper <> CStr(DR(0)("STATUS")).ToUpper Then
    ''                    If CStr(DR(0)("STATUS")).ToUpper = "RETIREE" Then
    ''                        '' get the retireeplan from elig_retire_elements and update eligcalc, mthdtl
    ''                        Dim memtype As String = "" : Dim recordcount As Short
    ''                        RegMasterDAL.UpdateEligCalelementsASStatusChange(_familyID, memtype, recordcount, Transaction)

    ''                        vWS_LOOKUP_MEMTYPE = memtype
    ''                        vMTH_MEMTYPE = memtype
    ''                        vRETURNED_ROW_COUNT = recordcount

    ''                        '' iF WE CHANGE THE MEMTYPE WE ALSO NEED TO CHANGE THE PLANTYPE

    ''                        If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then
    ''                            Dim DRmem As DataRow() = _memtype.Select(" MEMTYPE = '" & vWS_LOOKUP_MEMTYPE & "'  AND STATUS = '" & CStr(DR(0)("STATUS")).ToUpper & "' AND '" & CStr(Format(_eligPeriod, "yyyy-MM-dd")) & "' >= EFF_FROM_DATE AND '" & CStr(Format(_eligPeriod, "yyyy-MM-dd")) & "' <= EFF_TO_DATE")
    ''                            If DRmem.Count > 0 Then
    ''                                vMTH_PLANTYPE = CStr(DRmem(0)("PLANTYPE"))
    ''                            End If
    ''                        End If

    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN ELGCALC ELEMENTS ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            ''updated
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "ELGCALC ELEMENTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return False
    ''                        End If

    ''                    End If
    ''                End If


    ''                ''
    ''                ''

    ''                vWTHRLU_STATUS = CStr(DR(0)("STATUS"))

    ''                If (vENROLLED_IN_COBRA = 1) AndAlso (vELGACCTHRHOURS_CODE = "B") Then
    ''                    vWTHRLU_DEN_ELIG_SW = 0
    ''                End If
    ''            Else
    ''                vWTHRLU_MED_ELIG_SW = 0
    ''                vWTHRLU_DEN_ELIG_SW = 0
    ''                vWTHRLU_STATUS = "UNKNOWN"
    ''                MessageBox.Show("FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE" & Environment.NewLine & "PLEASE NOTIFY IS DEPARTMENT AS IT IS CRITICAL TO CALCULATE ELIGIBILITY ", "WEIGHTED_HOURS_LOOKUP", MessageBoxButtons.OK, MessageBoxIcon.Error)
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILED TO FIND MATCH IN WEIGHTED_HOURS_LOOKUP TABLE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            End If

    ''            ''---TW00-EXIT

    ''            If CStr(DR(0)("STATUS")) <> vMTH_STATUS Then
    ''                vSTATUS_IS_UNCHANGED = 1
    ''            End If

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ASSIGNING MED_ELIG_SW, DEN_ELIG_SW VALUES", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTH_MED_ELG_SW = vWTHRLU_MED_ELIG_SW      '' from   WEIGHTED_HOURS_LOOKUP table
    ''            vMTH_DEN_ELG_SW = vWTHRLU_DEN_ELIG_SW
    ''            vMTH_STATUS = vWTHRLU_STATUS
    ''        End If                 '' End of Incomplete


    ''        '' IF STATUS-IS-UNCHANGED
    ''        ''   GO TO 7000-CONTINUE
    ''        ''END-IF.


    ''        If vSTATUS_IS_UNCHANGED = 1 Then


    ''            ''--- ELGMTHDTL-STATUS IS SET IN TW00- PARAGRAPH.
    ''            ''--- WHEN WE GET HERE WE MUST DETERMINE WHAT KIND OF COBRA
    ''            ''--- AND CHANGE THE MEMTYPE ON THE ROW TO REFLECT IT'S
    ''            ''--- COBRA EQUIVALENT.  THIS IS NECESSARY FOR TRANSITIONAL
    ''            ''--- PROCESSING. ONCE CONVERTED, THE LAST-MEMTYPE ON
    ''            ''--- ELGCALC ELEMENTS WILL ALREADY BE SET TO COBRA
    ''            ''------  EVALUATE ELGMTHDTL-STATUS-TEXT

    ''            If vMTH_STATUS.ToUpper = "COBRA" Then
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ELGMTHDTL-STATUS-TEXT -- STAUS COBRA ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                ''     ---CB00-GET-A-OR-B-COBRA-DATA   THRU CB00-EXIT
    ''                ''--- IT IS NOT ENOUGH TO JUST BE ENROLLED IN COBRA TO
    ''                ''--- BE ELIGIBLE. THE PAYMENT MUST HAVE ALSO BEEN RECEIVED
    ''                ''--- AND POSTED. WHEN THE WEIGHTED HOURS IS NEGATIVE
    ''                ''---	CB00-GET-A-OR-B-COBRA-DATA.

    ''                If (_cobraactive IsNot Nothing) AndAlso (_cobraactive.Rows.Count > 0) Then
    ''                    ''  --- UPDATE THE DENTAL SWITCH IF THE PLAN CODE IS FOUND  AND IS AN 'A'.
    ''                    If CStr(_cobraactive.Rows(0)("HOURS_CODE")) = "A" Then
    ''                        vMTH_MED_ELG_SW = 1
    ''                        vMTH_DEN_ELG_SW = 1

    ''                    ElseIf CStr(_cobraactive.Rows(0)("HOURS_CODE")) = "B" Then

    ''                        vMTH_MED_ELG_SW = 1
    ''                        vMTH_DEN_ELG_SW = 0
    ''                    End If
    ''                Else ''--------------NO ROWS IN  _cobraactive
    ''                    vMTH_MED_ELG_SW = 0
    ''                    vMTH_DEN_ELG_SW = 0
    ''                End If
    ''                ''  ----- THRU CB00-EXIT


    ''                ''--- MOVE ELGCALC-LAST-MEMTYPE TO WS-LOOKUP-MEMTYPE
    ''                vWS_LOOKUP_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " GET MEMTYPE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''                ''---- PERFORM MX00-GET-COBRA-MEMTYPE-XREF   THRU MX00-EXIT

    ''                '' --- IF THE MEMTYPE WE ARE LOOKING UP IN THIS PARAGRAPH
    ''                '' --- IS ALREADY A COBRA MEMTYPE, THEN WE WILL NOT GET A
    ''                '' --- HIT AND WE WILL MAINTAIN THE EXISTING MEMTYPE.
    ''                '' --- WS-LOOKUP-MEMTYPE IS THE EXISTING MEMTYPE IN ELGCALC
    ''                '' --- ELEMENTS. THIS IS ONLY EXECUTED IF THE STATUS IS COBRA.
    ''                If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''                    Dim DR As DataRow() = _memtype.Select(" MEMTYPE = '" & vWS_LOOKUP_MEMTYPE & "'  AND STATUS IN ('ACTIVE','RETIREE') ")
    ''                    If DR.Count > 0 Then

    ''                        ''---MX10-UPDATE-ELGCALC-MEMTYPE  THRU MX10-EXIT

    ''                        vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateEligCalelementsfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN ELGCALC ELEMENTS ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            ''updated
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "ELGCALC ELEMENTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return False
    ''                        End If
    ''                        ''	---- THRU MX10-EXIT



    ''                        ''---EVALUATE WS-XREF-PLANTYPE
    ''                        If CStr(_memtype.Rows(0)("ELIG_RANKING")).ToUpper = "PLAN110" Then
    ''                            '' ----	    PERFORM MX20-UPDATE-A2COUNT-MEMTYPE      THRU MX20-EXIT
    ''                            vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                            If vRETURNED_ROW_COUNT = 1 Then
    ''                                ''updated
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Else
    ''                                MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                                Return False
    ''                            End If
    ''                        End If
    ''                        ''---MX20-EXIT.

    ''                    Else
    ''                        '' no rows in _memtype select
    ''                        ''-- MOVE WS-LOOKUP-MEMTYPE TO WS-XREF-MEMTYPE
    ''                        vWS_XREF_MEMTYPE = vWS_LOOKUP_MEMTYPE
    ''                    End If
    ''                    ''  THRU MX00-EXIT
    ''                    vMTH_MEMTYPE = vWS_XREF_MEMTYPE
    ''                End If



    ''            ElseIf vMTH_STATUS.ToUpper = "ACTIVE" Then
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " ELGMTHDTL-STATUS-TEXT -- STAUS ACTIVE ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                ''--- MOVE ELGCALC-LAST-MEMTYPE TO WS-LOOKUP-MEMTYPE
    ''                vWS_LOOKUP_MEMTYPE = CStr(_dreligcalcElements("LAST_MEMTYPE"))
    ''                ''--- PERFORM MX01-GET-ACTIVE-MEMTYPE-XREF  THRU MX01-EXIT

    ''                ''					 --- IF THE MEMTYPE WE ARE LOOKING UP IN THIS PARAGRAPH
    ''                ''					 --- IS ALREADY AN ACTIVE MEMTYPE, THEN WE WILL NOT GET A
    ''                ''					 --- HIT AND WE WILL MAINTAIN THE EXISTING MEMTYPE.
    ''                ''					 --- WS-LOOKUP-MEMTYPE IS THE EXISTING MEMTYPE IN ELGCALC
    ''                ''					 --- ELEMENTS. THIS IS ONLY EXECUTED IF THE STATUS IS ACTIVE.
    ''                ''					 --- AND ONLY IF THERE HAS BEEN A CHANGE IN STATUS.


    ''                If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''                    Dim DR As DataRow() = _memtype.Select(" MEMTYPE ='" & vWS_LOOKUP_MEMTYPE & "' AND STATUS IN ('COBRA','RETIREE') ")
    ''                    If DR.Count > 0 Then

    ''                        ''---MX10-UPDATE-ELGCALC-MEMTYPE  THRU MX10-EXIT

    ''                        vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateEligCalelementsfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                        If vRETURNED_ROW_COUNT = 1 Then
    ''                            ''updated
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Else
    ''                            MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "ELGCALC ELEMENTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN ELGCALC ELEMENTS", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Return False
    ''                        End If
    ''                        ''	---- THRU MX10-EXIT



    ''                        ''---EVALUATE WS-XREF-PLANTYPE
    ''                        If CStr(_memtype.Rows(0)("ELIG_RANKING")).ToUpper = "PLAN110" Then
    ''                            '' ----	    PERFORM MX20-UPDATE-A2COUNT-MEMTYPE      THRU MX20-EXIT
    ''                            vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, vWS_LOOKUP_MEMTYPE, Transaction))

    ''                            If vRETURNED_ROW_COUNT = 1 Then
    ''                                ''updated
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                            Else
    ''                                MessageBox.Show("FAILURE TO UPDATE MEMTYPE IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE MEMTYPE IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                                Return False
    ''                            End If
    ''                        End If
    ''                        ''---MX20-EXIT.

    ''                    Else
    ''                        '' no rows in _memtype select
    ''                        ''-- MOVE WS-LOOKUP-MEMTYPE TO WS-XREF-MEMTYPE
    ''                        vWS_XREF_MEMTYPE = vWS_LOOKUP_MEMTYPE
    ''                    End If

    ''                    '' THRU MX01-EXIT
    ''                    vMTH_MEMTYPE = vWS_XREF_MEMTYPE
    ''                End If

    ''            End If  ''IF vMTH_STATUS = 'COBRA' THEN STATEMENT
    ''            ''------   END-EVALUATE  ELGMTHDTL-STATUS-TEXT
    ''        End If            ''end if of If vSTATUS_IS_UNCHANGED = 1 Then


    ''        If CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "PLAN110" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---PLAN110 ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")

    ''            ''---  PERFORM A200-GET-A2COUNT          THRU A200-EXIT
    ''            ''   -- FOR PROCESSING OF HOURS THAT ARE RECEIVED AFTER THE
    ''            ''   -- A2BKCNT-OLDESTHRS DATE WE DO THE FOLLOWING:
    ''            ''   -- WHEN DETERMINING THE A2COUNT FOR A ROW, FIRST CHECK TO SEE IF
    ''            ''   -- THE PARTICIPANT IS BROKEN. IF SO, THEN THE COUNT WILL BE SET
    ''            ''   -- TO 3 AND THE OLDEST HOURS SET TO THE WORK PERIOD BEING
    ''            ''   -- PROCESSED.
    ''            ''   -- WHEN PROCESSING ROWS THAT PRECEEDE THE A2BKCNT-OLDESTHRS DATE
    ''            ''   -- WE SET THE A2COUNT IN MONTHLY DETAIL TO ZERO AND EXIT.


    ''            If (_calcA2cnt IsNot Nothing) AndAlso (_calcA2cnt.Rows.Count = 0) Then

    ''                vMTH_A2COUNT = 0
    ''            Else
    ''                ''   -- IF WE GOT A SUCCESSFUL RETURN CODE, WE NEED TO
    ''                ''-- DETERMINE IF WE ARE PROCESSING A PERIOD PRIOR TO
    ''                ''-- OUR OLDEST HOURS. IF THE OLDEST HOURS IS THE
    ''                ''-- DEFAULT OF 9999-12-31 OR PRECEEDES OUR OLDESTHRS
    ''                ''-- THEN WE CAN'T ACCURATELY DETERMINE THE A2COUNT
    ''                ''-- SO WE GO TO THE EXIT. A2BKCNT-OLDESTHRS WAS
    ''                ''-- ACQUIRED IN PARAGRAPH 1000-DECLARE-OPEN-ELGCURS.
    ''                If CDate(_dreligcalcElements("OLDESTHRS")) = CDate("9999-12-31") OrElse _eligPeriod < CDate(_dreligcalcElements("OLDESTHRS")) Then
    ''                    vMTH_A2COUNT = 0
    ''                End If

    ''            End If

    ''            ''------ EVALUATE MEMPLAN-STATUS-TEXT
    ''            If CStr(_dreligcalcElements("STATUS")).ToUpper = "ACTIVE" Then
    ''                If CInt(_calcA2cnt.Rows(0)("A2COUNT")) = 0 Then
    ''                    ''------ PERFORM A210-RESET-PLAN110     THRU A210-EXIT
    ''                    ''			-- THIS WILL UPDATE THE A2COUNT AND OLDEST HOURS FOR A
    ''                    ''			-- PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE

    ''                    vRETURNED_ROW_COUNT = CShort(RegMasterDAL.UpdateA2KKCNTfromEligCalculation(_familyID, _eligPeriod, Transaction))

    ''                    If vRETURNED_ROW_COUNT = 1 Then
    ''                        ''updated
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " UPDATED A2COUNT IN A2BKCNT .. PARTICIPANT WHO HAS RETURNED FROM A BREAK IN SERVICE", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                    Else
    ''                        MessageBox.Show("FAILURE TO UPDATE A2COUNT IN A2BKCNT" & Environment.NewLine & "  PLEASE CONTACT IS DEPARTMENT FOR SUPPORT. ", "A2BKCNT", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''                        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " FAILURE TO UPDATE A2COUNT IN A2BKCNT", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                        Return False
    ''                    End If
    ''                End If
    ''                ''----THRU A210-EXIT
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("A2COUNT"))
    ''            ElseIf CStr(_dreligcalcElements("STATUS")).ToUpper = "COBRA" Then
    ''                vMTH_A2COUNT = CInt(_calcA2cnt.Rows(0)("A2COUNT"))

    ''            Else
    ''                vMTH_A2COUNT = 0
    ''            End If
    ''            ''------ END-EVALUATE.

    ''            ''  ---- THRU A200-EXIT
    ''        ElseIf CStr(_dreligcalcElements("ELIG_RANKING")).ToUpper = "CURRENT" Then
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " EVALUATE MEMPLAN-ELIG-RANKING-TEXT---CURRENT ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            vMTHPLAN_AB_1ST = CDate(_drweightedHours("PLAN_AB_1ST_ELGDATE"))
    ''        Else
    ''            vMTHPLAN_AB_1ST = CDate("9999-12-31")
    ''            vMTH_A2COUNT = 0
    ''        End If
    ''        ''--- END-EVALUATE MEMPLAN-ELIG-RANKING-TEXT

    ''        ''---- PERFORM MD00-GET-MED-AND-DEN-PLANS      THRU MD00-EXIT.
    ''        If (_meddenplan IsNot Nothing) AndAlso (_meddenplan.Rows.Count > 0) Then
    ''            vMTH_MED_PLAN = CInt(_meddenplan.Rows(0)("MED_COVERAGE"))
    ''            vMTH_DEN_PLAN = CInt(_meddenplan.Rows(0)("DENT_COVERAGE"))

    ''        Else
    ''            vMTH_MED_PLAN = 0
    ''            vMTH_DEN_PLAN = 0

    ''        End If
    ''        ''------THRU MD00-EXIT.

    ''        ''----- PERFORM CP00-CHECK-PREMIUM-SW          THRU CP00-EXIT.
    ''        If (_memtype IsNot Nothing) AndAlso (_memtype.Rows.Count > 0) Then

    ''            Dim DR As DataRow() = _memtype.Select(" MEMTYPE ='" & vMTH_MEMTYPE.Trim & "'")
    ''            If DR.Count > 0 Then

    ''                If CStr(DR(0)("PREMREQ")).ToUpper = "N" Then
    ''                    vMTH_PREMIUM_SW = 0
    ''                ElseIf CStr(DR(0)("PREMREQ")).ToUpper = "Y" Then
    ''                    vMTH_PREMIUM_SW = 1
    ''                Else
    ''                    vMTH_PREMIUM_SW = 1
    ''                End If

    ''            Else
    ''                vMTH_PREMIUM_SW = 1
    ''            End If
    ''        End If
    ''        ''----  THRU CP00-EXIT.

    ''        ''---- PERFORM RP00-GET-RET-PLAN               THRU RP00-EXIT.
    ''        If (_retplan IsNot Nothing) AndAlso (_retplan.Rows.Count > 0) Then
    ''            vMTH_RET_PLAN = CStr(_retplan.Rows(0)("RETPLAN"))
    ''        Else
    ''            vMTH_RET_PLAN = ""
    ''        End If
    ''        ''---  THRU RP00-EXIT.



    ''        ''--- FOR THE CURRENT PERIOD ONLY, PLACE THE MEMTYPE
    ''        ''--- AND THE PLANTYPE ON MONTHLY DETAIL
    ''        ''--- FOR PRIOR PERIODS, LEAVE AS IS.
    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " completed updatable values method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''        Return True

    ''        ''---7000-UPDATE-MTHDTL


    ''    Catch ex As Exception
    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        End If
    ''        Return False
    ''    Finally

    ''    End Try
    ''End Function

    ''Public Function SaveChanges(ByRef Transaction As DbTransaction) As Boolean
    ''    Dim returnedstatus As Boolean = False
    ''    Try
    ''        'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " We are in SaveChanges method", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''        If (_mthdtlcount) IsNot Nothing AndAlso (CInt(_mthdtlcount.Rows(0)(0)) = 0) Then

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Checking the MTHDTL Row count", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            ''No row in the MTH_DTL Table
    ''            If _eligPeriod = RegMasterDAL._GlobalEligPeriod Then
    ''                '' Insert row for current period
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " No Row found in MTHDTL and Inserting row for current Period", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                returnedstatus = getcurrentvalues(Transaction)

    ''            Else
    ''                '' Insert row for Prior period
    ''                'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " No Row found in MTHDTL and Inserting row for prior Period", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''                returnedstatus = getpriorvalues(Transaction)
    ''            End If

    ''        Else
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Row found in MTHDTL and Updating the existing row", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            returnedstatus = updatablevalues(Transaction)
    ''        End If

    ''    Catch ex As Exception
    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        Else
    ''            returnedstatus = False
    ''        End If
    ''    End Try

    ''    If returnedstatus Then

    ''        Dim HistSum As String = ""
    ''        Dim HistDetail As String = ""
    ''        Dim user As String = RegMasterDAL._DomainUser

    ''        Try

    ''            HistSum = "ELIGIBILTY WAS CALCULATED FOR THE FAMILYID: " & CStr(_familyID)
    ''            HistDetail = RegMasterDAL.General.DomainUser.ToUpper & " CALCULATED THE ELIGIBILITY  FOR THE PERIOD " & CStr(_eligPeriod)

    ''            If (vMTH_STATUS) IsNot Nothing Then

    ''                RegMasterDAL.InsertMTHDTL(_familyID, _eligPeriod, vMTH_STATUS, vMTH_PLANTYPE, vMTH_MEMTYPE, vMTH_LOCALNO, vMTH_MED_PLAN,
    ''                                       vMTH_DEN_PLAN, vMTH_MED_ELG_SW, vMTH_DEN_ELG_SW, vMTH_PREMIUM_SW, vMTH_FAMILY_SW, vMTH_RET_PLAN, CShort(vMTH_A2COUNT), vMTHPLAN_AB_1ST, vMTH_BREAK_IN_SERVICE_SW, Transaction)

    ''            End If
    ''            RegMasterDAL.CreateRegHistory(_familyID, 0, Nothing, Nothing, "ELIGCALCULATION", Nothing, Nothing, Nothing, HistSum, HistDetail, user.ToUpper, Transaction)

    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Inserted/Updated MTHDTL ", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")
    ''            'If _TraceSwitch.Enabled Then CMSDALLog.Log(Now.ToString("HH:mm:ss.fff") & " Added entry to Reg History table", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Now.Year) & String.Format("{00}", Now.Month) & "CalcEligibility.txt")

    ''            Return True

    ''        Catch ex As Exception

    ''            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''            If (rethrow) Then
    ''                Throw
    ''            Else
    ''                Return False
    ''            End If
    ''        End Try
    ''    Else
    ''        Return False   '' above procedures return false
    ''    End If
    ''End Function

    Public Sub disposeobjects()
        If _WeightedHours IsNot Nothing Then
            _WeightedHours.Dispose()
            _WeightedHours = Nothing
        End If

        If _EligCalcElements IsNot Nothing Then
            _EligCalcElements.Dispose()
            _EligCalcElements = Nothing
        End If
        ''If _mthdtlcount IsNot Nothing Then
        ''    _mthdtlcount.Dispose()
        ''    _mthdtlcount = Nothing
        ''End If
        ''If _term IsNot Nothing Then
        ''    _term.Dispose()
        ''    _term = Nothing
        ''End If
        ''If _nhw IsNot Nothing Then
        ''    _nhw.Dispose()
        ''    _nhw = Nothing
        ''End If
        ''If _cobrapayment IsNot Nothing Then
        ''    _cobrapayment.Dispose()
        ''    _cobrapayment = Nothing
        ''End If
        ''If _negativerows IsNot Nothing Then
        ''    _negativerows.Dispose()
        ''    _negativerows = Nothing
        ''End If

        ''If _weightedhrslookup IsNot Nothing Then
        ''    _weightedhrslookup.Dispose()
        ''    _weightedhrslookup = Nothing
        ''End If
        ''If _cobraactive IsNot Nothing Then
        ''    _cobraactive.Dispose()
        ''    _cobraactive = Nothing
        ''End If
        ''If _memtype IsNot Nothing Then
        ''    _memtype.Dispose()
        ''    _memtype = Nothing
        ''End If
        ''If _calcA2cnt IsNot Nothing Then
        ''    _calcA2cnt.Dispose()
        ''    _calcA2cnt = Nothing
        ''End If
        ''If _meddenplan IsNot Nothing Then
        ''    _meddenplan.Dispose()
        ''    _meddenplan = Nothing
        ''End If
        ''If _retplan IsNot Nothing Then
        ''    _retplan.Dispose()
        ''    _retplan = Nothing
        ''End If
        ''If _retromemtype IsNot Nothing Then
        ''    _retromemtype.Dispose()
        ''    _retromemtype = Nothing
        ''End If
        ''If _cobraqe IsNot Nothing Then
        ''    _cobraqe.Dispose()
        ''    _cobraqe = Nothing
        ''End If
        ''If _retireeacctno IsNot Nothing Then
        ''    _retireeacctno.Dispose()
        ''    _retireeacctno = Nothing
        ''End If

        If _DataDS IsNot Nothing Then
            _DataDS.Dispose()
            _DataDS = Nothing
        End If

    End Sub

    Public Function ResetSwitchesForEligcalculation(ByVal familyid As Integer, ByVal eligPeriod As Date) As Boolean

        Try
            '' set  statuses on rows for elig Calculation
            RegMasterDAL.ResetswitchesforEligcalculation(_familyID, _eligPeriod)

            Return True
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                Return False
            End If
        End Try

    End Function

    Public Function DetermineEligibility(ByVal familyid As Integer, ByVal eligPeriod As Date) As Boolean
        Dim Status As Boolean = False
        Dim Transaction As DbTransaction

        Try
            _DataDS = RegMasterDAL.RetrieveCommonDataforEligcalculation(_familyID, _eligPeriod)   '' Retrieve require common data from other tables

            If _DataDS IsNot Nothing AndAlso _DataDS.Tables.Count > 0 Then
                _EligCalcElements = _DataDS.Tables(0)
            Else
                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before calculation Eligibility.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            '' No row from elig cal elements

            If (_EligCalcElements IsNot Nothing) AndAlso (_EligCalcElements.Rows.Count > 0) Then
                _dreligcalcElements = _EligCalcElements.Rows(0)
            Else
                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before calculation of Eligibility.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Transaction = RegMasterDAL.BeginTransaction

            If SaveChanges(Transaction) = True Then
                RegMasterDAL.CommitTransaction(Transaction)
                Status = True
            Else
                RegMasterDAL.RollbackTransaction(Transaction)
                Status = False
            End If

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                RegMasterDAL.RollbackTransaction(Transaction)
            End If

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                Status = False
            End If
        Finally
            If Transaction IsNot Nothing Then
                Transaction.Dispose()
                Transaction = Nothing
            End If
            disposeobjects()
        End Try
        Return Status
    End Function

    Public Function SaveChanges(ByRef Transaction As DbTransaction) As Boolean
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim user As String = UFCWGeneral.WindowsUserID.Name

        Dim returnedcode As Integer = 1

        Try

            HistSum = "ELIGIBILTY WAS CALCULATED FOR THE FAMILYID: " & CStr(_familyID)
            HistDetail = user.ToUpper & " CALCULATED THE ELIGIBILITY  "   ''FOR THE PERIOD " & CStr(_eligPeriod)

            returnedcode = RegMasterDAL.CalculateEligibility(_familyID, CStr(_dreligcalcElements("LAST_MEMTYPE")), CShort(_dreligcalcElements("LAST_LOCAL")), Transaction)

            RegMasterDAL.CreateRegHistory(_familyID, 0, Nothing, Nothing, "ELIGCALCULATION", Nothing, Nothing, Nothing, HistSum, HistDetail, user.ToUpper, Transaction)

            If returnedcode = 0 Then

                RegMasterDAL.UpdateSwitchesAfterEligCalculation(_familyID, _eligPeriod, Transaction)   ''set the process status P

                Return True
            Else
                Return False
            End If

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                Return False
            End If
        End Try
    End Function

#End Region

End Class
