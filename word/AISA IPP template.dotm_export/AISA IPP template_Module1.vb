Attribute VB_Name = "Module1"
Public Const ShowQuestionNo = False  'could be True or False
'----------------------------------   -------------------------------------------
' Developed for DIA
' Created by:       Allfields Customised Solutions Limited
' Contact Info:     hello@allfields.co.nz, 04 978 7101
' Date:             October 2017
' Description:      Implement decision making tree for IPP
'-----------------------------------------------------------------------------

Public aryOptionButtons() As New OptionButtonEvent  'arrry to hold option button object in main frame
Public sSelectedCaption As String       'caption of selected option button in first screen
Public aryNodes() As New oNode
Public aryNodeCnt As Integer
Public sHelpText As String
Public sCurrent As String   'current oNode name
Public sNextNode As String  'next node name
Public sPreNode As String   'previous node name
Public EnableYesNoEvent As Boolean
Public Const DefaultAnswerText = "You can complete this in full in the resulting word document."
Public Const PlaceHolderText = "Space to write more"
Public Const QuestionStyle = "Question" 'Word style name for question
Public Const AnswerStyle = "Answer"     'Word style name for yes/no answer
Public Const FirstNode = "51"            'name of node to start with
Public Const PreFixString = "Your choice(s) indicate that:  Disclosure is permitted ("  'prefix wording for IPP exception clauses

Function InitialNodes()
    ReDim aryNodes(0)
    aryNodeCnt = 0
    '### Permitting and Exit node
    CreateNode Name:="permitted", _
                Question:="Your application is permitted.", _
                YesNode:="", _
                NoNode:="", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="exit", _
                Question:="Your application is not permitted!", _
                YesNode:="", _
                NoNode:="", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
    
    'Normal nodes
    CreateNode Name:="1", _
                Question:="You are not sure if an IPP exception applies or perhaps are not sure which one might apply. First we will look at your purpose for disclosing the information in light of why it was collected (IPP 11(a))." & vbNewLine & "You should start by making a note of what you believe was the original purpose of collection. The final Word document will contain what you type here.", _
                YesNode:="2", _
                NoNode:="2", _
                NeedAnswer:=True, _
                Tip:="Purpose could be redifined to include disclosure for future information holdings (can not be applied retrospectively).", _
                Answer:="", _
                ActionNo:=0, _
                NextNode:="2"
                
    CreateNode Name:="2", _
                Question:="Was this purpose communicated to the individual concerned at the time of collection?", _
                YesNode:="3", _
                NoNode:="exit", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="For example:" & vbNewLine & Chr(149) & "There is a statement on our arrival form that explains this type of disclosure, or" & vbNewLine & Chr(149) & "We have a legal opinion that this type of disclosure  is implicit in the purpose for collection", _
                ActionNo:=2, _
                NoText:="Check collection processes for IPP3 compliance. If the collection was compliant with IPP3, despite not being communicated to the individual at the time of collection, you may still be able to use an exception."
                
    CreateNode Name:="3", _
                Question:="Now we need to record why you believe that the purpose for disclosure is a purpose for which the information was collected. What you write here will be in the final Word document. " & vbNewLine & "I have reasonable grounds to believe the disclosure is a purpose for collecting the information because:", _
                YesNode:="Warning_a", _
                NoNode:="4", _
                NeedAnswer:=True, _
                Tip:="Remember that an explanation devised in hindsight won't suffice.", _
                Answer:="", _
                ActionNo:=2
    
    'warning box: a
    CreateNode Name:="Warning_a", _
                Question:="Disclosure is permitted under IPP11(a) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="4", _
                Question:="Now we need to look at whether the purpose for disclosure is 'directly related' to a purpose for collection." & vbNewLine & "I have reasonable grounds for believing that the disclosure is directly related to the purpose for which the information was originally collected, because:", _
                YesNode:="Warning_a", _
                NoNode:="5", _
                NeedAnswer:=True, _
                Tip:="Whether or not a purpose included disclosure, or whether a disclosure is directly related to the purposeis a question of fact." & vbNewLine & "(Director of Human Rights Proceedings v Crampton [2015] NZHRRT 35 at [81-82])" & vbNewLine & "That makes it advisable to document the purpose for collecting, obtaining, or creating information, and to note the reasons for disclosing it.", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(a))"
                
    CreateNode Name:="5", _
                Question:="Will you be making the disclosure to the individual concerned?", _
                YesNode:="Warning_c", _
                NoNode:="6", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(c))"
                
    'warning box: c
    CreateNode Name:="Warning_c", _
                Question:="Disclosure is permitted under IPP11(c) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="6", _
                Question:="Is the disclosure authorised by the individual concerned?", _
                YesNode:="Warning_d", _
                NoNode:="7", _
                NeedAnswer:=False, _
                Tip:="How recent is the authorisation?" & vbNewLine & "Should a new authorisation be sought?", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(d))"

    'warning box: d
    CreateNode Name:="Warning_d", _
                Question:="Disclosure is permitted under IPP11(d) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="7", _
                Question:="Does the information come from a publicly available publication?", _
                YesNode:="Warning_b", _
                NoNode:="8", _
                NeedAnswer:=False, _
                Tip:="E.g. a website, news story, public register. The information being disclosed must actually come from that public source", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(b))"

    'warning box: b
    CreateNode Name:="Warning_b", _
                Question:="Disclosure is permitted under IPP11(b) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="8", _
                Question:="Is it going to be used in a way that will indentify the individual?", _
                YesNode:="9", _
                NoNode:="Warning_hi", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                NoTextBox:=True, _
                NoText:=PreFixString & "IPP 11(h)(i))"
    
    'warning box: h/i
    CreateNode Name:="Warning_hi", _
                Question:="Disclosure is permitted under IPP11(h)(i) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
    
    '### this node needs a 'No' branch
    CreateNode Name:="9", _
                Question:="Is the information going to be used for statistical or research purposes? " & vbNewLine & "If it is, will the published research identify individuals?", _
                YesNode:="10", _
                NoNode:="Warning_hii", _
                NeedAnswer:=False, _
                Tip:="Information does not have to be de-identified at point of disclosure, as long as the published research doesn't identify individuals.", _
                Answer:="", _
                ActionNo:=2, _
                NoTextBox:=True, _
                NoText:=PreFixString & "IPP 11())"
                
    'warning box: h/ii
    CreateNode Name:="Warning_hii", _
                Question:="Disclosure is permitted under IPP11(h)(ii) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="10", _
                Question:="Will the published research identify individuals?", _
                YesNode:="11", _
                NoNode:="Warning_hii", _
                NeedAnswer:=False, _
                Tip:="May need something here about what identification means...", _
                Answer:="", _
                ActionNo:=2, _
                NoTextBox:=True, _
                NoText:=PreFixString & "IPP 11(h)(ii))"

    CreateNode Name:="11", _
                Question:="Do the individual consent to the disclosure?", _
                YesNode:="Warning_a", _
                NoNode:="12", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(a))"

    CreateNode Name:="12", _
                Question:="Is disclosure part of the sale or disposition of a business as a going concern?", _
                YesNode:="Warning_g", _
                NoNode:="13", _
                NeedAnswer:=False, _
                Tip:="E.g. the sale of a retail business or a professional firm (e.g. law firm, accountancy) includes its customer list." & vbNewLine & " This exception DOES NOT permit:" & vbNewLine & Chr(149) & " Sale of a customer list without the business also being sold." & vbNewLine & Chr(149) & " Sale of a customer list to defray debts in a receivership or liquidation.", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(g))"
    
    'warning box: g
    CreateNode Name:="Warning_g", _
                Question:="Disclosure is permitted under IPP11(g) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="13", _
                Question:="Has the Privacy Commissioner authorised me the disclosure the information?", _
                YesNode:="Warning_i", _
                NoNode:="14", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesTextBox:=True, _
                YesText:=PreFixString & "IPP 11(i))"
    
    'warning box: i
    CreateNode Name:="Warning_i", _
                Question:="Disclosure is permitted under IPP11(i) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="14", _
                Question:="Is disclosure necessary to avoid prejudice to maintenance of the law?" & vbNewLine & "If you think it might be, you need to record what law is and what agency enforces it.", _
                YesNode:="17", _
                NoNode:="17", _
                NeedAnswer:=False, _
                Tip:="Maintenance of the law includes:" & vbNewLine & Chr(149) & " Prevention - e.g." & vbNewLine & Chr(149) & " Detection - e.g. checking with an agency to see whether an employee has wrongfully accessed an information system, to verify allegations made (Tan v NZ Police [2016] NZHRRT 32)" & vbNewLine & Chr(149) & " Investigation - e.g." & vbNewLine & Chr(149) & " Prosecution - e.g." & vbNewLine & Chr(149) & " Punishment - e.g. disclosure details of fines to Ministry of Justice collections unit for enforcement.", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="17", _
                Question:="The Law in issue is:", _
                YesNode:="18", _
                NoNode:="18", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="18", _
                Question:="The law is enforced by:", _
                YesNode:="19", _
                NoNode:="19", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="19", _
                Question:="Is this a public sector agency.", _
                YesNode:="15", _
                NoNode:="20", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="15", _
                Question:="You need to record the reason you think the disclosure is necessary. " & vbNewLine & "I believe the disclosure is necessary because:", _
                YesNode:="16", _
                NoNode:="16", _
                NeedAnswer:=True, _
                Tip:="Necessary means �needed or required� in the circumstances, not just �desirable or expedient�. However, �needed or required� is something less than �indispensible or essential�." & vbNewLine & "(Tan v NZ Police [2016] NZHRRT 32 at [77]; Commissioner of Police v Director of Human Rights Proceedings (2007) HRNZ 364 at [53])", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="16", _
                Question:="You need to record why you believe that." & vbNewLine & "I have reasonable ground for my belief because:" & vbNewLine & "If you think your grounds are reasonable choose 'Yes'." & vbNewLine & "If you are uncertain, choose 'No'.", _
                YesNode:="Warning_ei", _
                NoNode:="20", _
                NeedAnswer:=True, _
                Tip:="It must be an action belief based on a proper consideration of the relevant circumstances", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(e)(i))"
    
    'warning box: ei
    CreateNode Name:="Warning_ei", _
                Question:="Disclosure is permitted under IPP11(e)(i) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="20", _
                Question:="Is disclosure necessary for enforcing a percuniary penalty?", _
                YesNode:="21", _
                NoNode:="24", _
                NeedAnswer:=False, _
                Tip:="Pecuniary penalties are monetary penalties imposed by statute. They are intended to punish and deter contravention of the law. They may be issued in civil or criminal proceedings." & vbNewLine & "Law Commission (2014). Pecuniary penalties: guidance for legislative design. NZLC R133, at p. 4.", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="21", _
                Question:="Now you need to record why you believe the disclosure is necessary." & vbNewLine & "I believe the disclosure is necessary because:", _
                YesNode:="22", _
                NoNode:="22", _
                NeedAnswer:=True, _
                Tip:="Necessary means 'needed or required' in the circumstances, not just desirable or expedient'. However, 'needed or required' is something less than 'indispensible or essential'." & vbNewLine & "(Tan v NZ Police [2016] NZHRRT 32 at [77]; Commissioner of Police v Director of Human Rights Proceedings (2007) HRNZ 364 at [53])", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="22", _
                Question:="You need to record why you believe that. " & vbNewLine & "I have reasonable ground for my belief because:" & vbNewLine & "If you think your grounds are reasonable choose 'Yes'." & vbNewLine & "If you are uncertain, choose 'No'.", _
                YesNode:="Warning_eii", _
                NoNode:="24", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(e)(ii))"
    
    'warning box: e/ii
    CreateNode Name:="Warning_eii", _
                Question:="Disclosure is permitted under IPP11(e)(ii) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

'    CreateNode Name:="23", _
'                Question:="There is no such law", _
'                YesNode:="24", _
'                NoNode:="24", _
'                NeedAnswer:=False, _
'                Tip:="", _
'                Answer:="", _
'                ActionNo:=0

    CreateNode Name:="24", _
                Question:="Is disclosure necessary for the protection of the public revenue?", _
                YesNode:="25", _
                NoNode:="26", _
                NeedAnswer:=False, _
                Tip:="E.g. disclosure is to assess tax liabilities, identify benefit fraud, enforce child support payments or payment of infringements or court fines.", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="25", _
                Question:="You need to record the specific public revenue that you believe is in issue." & vbNewLine & "The public revenue in issue is:", _
                YesNode:="27", _
                NoNode:="27", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="27", _
                Question:="I believe the disclosure is necessary because:", _
                YesNode:="28", _
                NoNode:="28", _
                NeedAnswer:=True, _
                Tip:="Necessary means 'needed or required' in the circumstances, not just 'desirable or expedient'. However, 'needed or required' is something less than 'indispensible or essential'." & vbNewLine & "(Tan v NZ Police [2016] NZHRRT 32 at [77]; Commissioner of Police v Director of Human Rights Proceedings (2007) HRNZ 364 at [53])", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="28", _
                Question:="I have reasonable ground for my belief because:" & vbNewLine & "(If you think your grounds are reasonable choose 'Yes'." & vbNewLine & "If you are uncertain, choose 'No'.", _
                YesNode:="Warning_eiii", _
                NoNode:="29", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(e)(iii))"
                
    CreateNode Name:="Warning_eiii", _
                Question:="Disclosure is permitted under IPP11(e)(iii) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="26", _
                Question:="The public revenue is not in issue:", _
                YesNode:="29", _
                NoNode:="29", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="29", _
                Question:="Is the disclosure necessary for the conduct of proceedings before any court or tribunal?", _
                YesNode:="30", _
                NoNode:="34", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="30", _
                Question:="Have the proceedings started or are 'reasonable in contemplation'", _
                YesNode:="31", _
                NoNode:="34", _
                NeedAnswer:=False, _
                Tip:="Reasonably in contemplation means [to come]...", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="31", _
                Question:="You need to identify what the proceedings are (or are expected to be):", _
                YesNode:="32", _
                NoNode:="32", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="32", _
                Question:="Now you need to explain why a disclosure is needed/necessary:" & vbNewLine & "I believe the disclosure is necessary because:", _
                YesNode:="33", _
                NoNode:="33", _
                NeedAnswer:=True, _
                Tip:="Necessary means 'needed or required' in the circumstances, not just 'desirable or expedient'." & vbNewLine & "However, 'needed or required' is something less than 'indispensible or essential'." & vbNewLine & "(Tan v NZ Police [2016] NZHRRT 32 at [77]; Commissioner of Police v Director of Human Rights Proceedings (2007) HRNZ 364 at [53])", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="33", _
                Question:="You need to record why you believe that." & vbNewLine & "I have reasonable ground for my belief because:" & vbNewLine & "If you think your grounds are reasonable choose 'Yes'." & vbNewLine & "If you are uncertain, choose 'No'.", _
                YesNode:="Warning_eiv", _
                NoNode:="34", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(e)(iv))"
    
    CreateNode Name:="Warning_eiv", _
                Question:="Disclosure is permitted under IPP11(e)(iv) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    '###need a no branch
    CreateNode Name:="34", _
                Question:="Is disclosure necessary to prevent or lessen a serious threat?", _
                YesNode:="35.1", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="EndpointBoxAA", _
                Question:="If none of the exceptions to IPP11 appear to give you permission to make the disclosure, some other authorisation will be needed for the disclosure (e.g. AISA, Schedule 4A or 5 entry, Privacy Act Code of Practice, bespoke legislation, s 54 authorisation)." & vbNewLine & "You may want to discuss your results with your agency's privacy officer or a member of your legal team.", _
                YesNode:="exit", _
                NoNode:="exit", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="35.1", _
                Question:="Is it a threat to public health or safety?", _
                YesNode:="36", _
                NoNode:="35.2", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="35.2", _
                Question:="Is it a threat to a specific person?", _
                YesNode:="36", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Return to the beginning to see if other exceptions apply. If not, authorisation will be needed for the disclosure."

    CreateNode Name:="36", _
                Question:="Is it a serious threat?" & vbNewLine & "If you think it is not serious choose 'No'. If you think it might be serious but aren't sure, choose 'Yes' and we'll help you decide.", _
                YesNode:="37", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=True, _
                Tip:="A 'serious' threat is one that the agency reasonably believes is serious based on three factors", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Return to the beginning to see if other exceptions apply. If not, authorisation will be needed for the disclosure."

    CreateNode Name:="37", _
                Question:="How likely is it that the threat will come to pass? Highly likely, possible or is there only a remote change? You can write your answer in the box.", _
                YesNode:="38", _
                NoNode:="38", _
                NeedAnswer:=True, _
                Tip:="Is the threat very likely to occur? Is it possible, or is there only a remote chance?", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="38", _
                Question:="How serious will the consequences be if the threat comes to pass? Things to think about:" & vbNewLine & "Will the consequences be felt by one person or many?" & vbNewLine & "Is anyone likely to die or be injured as a result of the threat?" & vbNewLine & "Are people likely to have their identities, financial details, or money stolen as a result of the threat?", _
                YesNode:="39", _
                NoNode:="39", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="39", _
                Question:="When is the threat likely to come to pass? Is it immediately or soon or not sure when?" & vbNewLine & "And more space to write your thoughts.", _
                YesNode:="40", _
                NoNode:="40", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="40", _
                Question:="I have reasonable grounds for my assessment that the threat is serious because: " & vbNewLine & "If after answering those questions you think that you do have good grounds, sum up your reasons and choose 'Yes'" & vbNewLine & "If your answer make you doubt that it is truly a serious threat, record that and choose 'No'", _
                YesNode:="41", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Return to the beginning to see if other exceptions apply. If not, authorisation will be needed for the disclosure."

    CreateNode Name:="41", _
                Question:="I believe the disclosure is necessary because: you need to record your reason for why you believe the disclosure is 'necesssary'. Case law gives 'necessary' a meaning here which may help you:" & vbNewLine & "1. Is the person/agency you will give the information to able to lessen/mitigate the harm of the threat? If no, then the disclosure may not be necessary." & vbNewLine & "2. Is the disclosure 'needed or required' and not just 'desirable or expedient'? But it doesn't have to be as strong as 'indispensable or essential'", _
                YesNode:="Warning_f", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=True, _
                Tip:="Necessary means 'needed or required' in the circumstances, not just 'desirable or expedient'." & vbNewLine & "However, 'needed or required' is something less than 'indispensible or essential'." & vbNewLine & "(Tan v NZ Police [2016] NZHRRT 32 at [77]; Commissioner of Police v Director of Human Rights Proceedings (2007) HRNZ 364 at [53])" & vbNewLine & "The disclosure needs to be made to a person who can do something to mitigate the threat. Otherwise, the disclosure may not be necessary." & vbNewLine & "(Henderson v Police Commissioner (HC WN CIV 2009-485-1037 29 April 2010 at [78])", _
                Answer:="", _
                ActionNo:=0
    
    CreateNode Name:="Warning_f", _
                Question:="Disclosure is permitted under IPP11(f) but you're not quite there yet.", _
                YesNode:="54", _
                NoNode:="54", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

                
    CreateNode Name:="42", _
                Question:="I have reasonable grounds for my belief because: ", _
                YesNode:="54", _
                NoNode:="EndpointBoxAA", _
                NeedAnswer:=True, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesText:=PreFixString & "IPP 11(f))", _
                NoText:="Return to the beginning to see if other exceptions apply. If not, authorisation will be needed for the disclosure (e.g. AISA, Schedule 4A or 5 entry, bespoke legislation."
    
    CreateNode Name:="51", _
                Question:="Does legislation other than the Privacy Act prevent or regulate disclosure? If you are not sure, choose 'No'.", _
                YesNode:="52", _
                NoNode:="53", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2
    
    CreateNode Name:="52", _
                Question:="Is information subject to Tax Administration Act, Senior Courts Act, District Courts Act, or Births, Deaths, Marriages, and Relationships Registration Act?", _
                YesNode:="52y", _
                NoNode:="52n", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                YesText:="In this case you will need an Approved Information Sharing Agreement (AISA) under Part 9A of the Privacy Act.", _
                NoText:="In that case you must comply with that law or seek an amendment."
                
    CreateNode Name:="52y", _
                Question:="In this case you will need an Approved Information Sharing Agreement (AISA) under Part 9A of the Privacy Act.", _
                YesNode:="exit", _
                NoNode:="exit", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0
                
    CreateNode Name:="52n", _
                Question:="In that case you must comply with that law or seek an amendment.", _
                YesNode:="exit", _
                NoNode:="exit", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=0

    CreateNode Name:="53", _
                Question:="Do any of the Information Privacy Principle (IPP) exceptions clearly apply?" & vbNewLine & "Choice buttons <No> and <Yes or Don't know>", _
                YesNode:="57", _
                NoNode:="exit", _
                NeedAnswer:=False, _
                Tip:="IPP 11 sets out a general rule that information should not be disclosed unless disclosure is one of the purposes for having the information in the first place." & vbNewLine & "IPP 11 sets out a range of exceptions to this general rule. In proceedings under the Privacy Act, defendants have to prove an exception applies so it makes sense to keep a record of the decision and its rationale.", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Other authorisation is needed (e.g. AISA, Schedule 4A or 5 entry, Privacy Act Code of Practice, bespoke legislation, s 54 authorisation)."
    
    'new node 13/11/2017
    CreateNode Name:="57", _
                Question:="If you know which IPP exception applies, choose 'YES', If you are not sure if an IPP exception applies or which exception it is, choose 'No'", _
                YesNode:="54", _
                NoNode:="1", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2

    CreateNode Name:="54", _
                Question:="You have decided that an exception to an Information Privacy Principles applies. Now you need to answer some questions about how you want to do it. Some ways of sharing information may not meet the requirements to use an exception." & vbNewLine & "1.  Have you established threshold criteria for this type of sharing? These might include the possible harm to an individual if the information is not disclosed." & vbNewLine & "2.  For bulk or automated releases, can the threshold criteria be applied automatically?" & vbNewLine & "If you can answer 'Yes' to BOTH questions choose 'Yes'" & vbNewLine & "If your answer to either question is 'No' or 'Don't know' choose 'No'.", _
                YesNode:="55", _
                NoNode:="permitted", _
                NeedAnswer:=False, _
                Tip:="Consider using a memorandum of understanding to: specify how information sharing will work;" & vbNewLine & "identify how accuracy of disclosed information will be ensured; specify how disclosed information will be used. Consider publishing memorandums of understanding to enhance transparency.", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Case-by-case disclosure will be required. This normally involves a human being making a decision."
                
    CreateNode Name:="55", _
                Question:="The exceptions to the IPPs do not allow you to share irrelevant information.  Can the information at issue be severed from any irrelevant information, so disclosure is limited to just the relevant information?", _
                YesNode:="56", _
                NoNode:="exit", _
                NeedAnswer:=False, _
                Tip:="", _
                Answer:="", _
                ActionNo:=2, _
                NoText:="Other authorisation is needed (e.g. AISA, Schedule 4A or 5 entry, Privacy Act Code of Practice, bespoke legislation, s 54 authorisation)."
    
    '###
    CreateNode Name:="56", _
                Question:="Personal information needs to be disclosed responsibly. When disclosing personal information you are expected to make reasonable efforts to minimise risks of harm to the individual." & vbNewLine & "Is there a risk that...", _
                YesNode:="exit", _
                NoNode:="permitted", _
                NeedAnswer:=False, _
                Tip:="Harm includes taking adverse action against a person (e.g. stopping a benefit, imposing a sanction). Adverse action is justifiable if based on accurate information, and accompanied by natural justice. Harm includes significant distress or humiliation, material losses, damage to reputation. Harm is particularly likely if information is inaccurate.", _
                Answer:="", _
                ActionNo:=2, _
                YesText:="Disclose in accordance with the IPP exception(s).", _
                NoText:="Reduce risk by building-in natural justice processes (if an adverse action may be taken) and safeguards to improve the accuracy and reliability of the information for the purpose for which it will be used."
                
End Function

Function CreateDocument(stage As String)
'create document based on selections
    Dim doc As Document
    Set doc = ActiveDocument
    fmNodes.Hide
    Dim bm As Bookmark
    Dim rg As Range
    doc.Paragraphs.Add
    Set rg = doc.Paragraphs.Last.Range
    If stage = "1" Then
    'finish at 1st pop up
        rg.Text = sSelectedCaption & vbNewLine & vbTab & "Yes." & vbNewLine & "As you believe that you have the authority to share the information, your responsibility is to do so in accordance with the requirements in the authorisation you have. If you need information on any of these things please see the resources list in the Toolkit. "
    Else
    'rest decision tree, spit out all questions and answer/choices
        Dim nd As oNode
        Set nd = GetNodeByName(FirstNode)
        Do While nd.Name <> "exit" And nd.Name <> "permitted"
            doc.Paragraphs.Add
            doc.Paragraphs.Last.Range.Text = nd.Name & ": " & nd.sQuestion '& vbNewLine & vbTab & IIf(nd.ActionNo > 0, IIf(nd.YesNo = "y", "Yes: ", "No."), "") & nd.sAnswer
            doc.Paragraphs.Last.Range.Style = QuestionStyle
            If nd.ActionNo > 0 Then
                doc.Paragraphs.Add
                doc.Paragraphs.Last.Range.Style = AnswerStyle
                doc.Paragraphs.Last.Range.Text = IIf(nd.ActionNo > 0, IIf(nd.YesNo = "y", "Yes: ", "No."), "")
            End If
            'for those needs a statement before next question
            If (nd.YesNo = "y" And nd.YesText <> "") Or (nd.YesNo = "n" And nd.NoText <> "") Then
                doc.Paragraphs.Add
                doc.Paragraphs.Last.Style = AnswerStyle
                doc.Paragraphs.Last.Range.Text = IIf(nd.YesNo = "y", nd.YesText, nd.NoText)
            End If
            If nd.NeedAnswer Or (nd.YesNo = "y" And nd.YesTextBox) Or (nd.YesNo = "n" And nd.NoTextBox) Then
                doc.Paragraphs.Add
                doc.Paragraphs.Last.Range.Style = AnswerStyle
                Set rg = doc.AttachedTemplate.BuildingBlockEntries("IPP_AnswerBox_Blank").Insert(doc.Paragraphs.Last.Range, True)
                If nd.sAnswer = "" Then
                    If rg.Tables.Count > 0 Then
                        rg.Tables(1).Cell(1, 1).Range.Text = PlaceHolderText
                    End If
                ElseIf nd.sAnswer <> DefaultAnswerText And Left(nd.sAnswer, 3) <> "IPP" Then
                    If rg.Tables.Count > 0 Then
                        rg.Tables(1).Cell(1, 1).Range.Text = nd.sAnswer
                    End If
                End If
                rg.Editors.Add wdEditorEveryone
            End If
            Set nd = GetNodeByName(nd.NextNode)
        Loop
        doc.Paragraphs.Add
        Set rg = doc.Content
        rg.Collapse wdCollapseEnd
        If nd.Name = "exit" Then
            rg.Text = "Your application is not permitted."
        Else
            rg.Text = "Your application is permitted."
        End If
    End If
    doc.Protect wdAllowOnlyReading
End Function

Function CreateNode(Name As String, Question As String, YesNode As String, _
                    NoNode As String, NeedAnswer As Boolean, Tip As String, _
                    Answer As String, ActionNo As Integer, Optional PreviousNode As String = "", _
                    Optional NextNode As String = "", Optional YesNo As String = "", _
                    Optional YesTextBox As Boolean = False, Optional NoTextBox As Boolean = False, _
                    Optional YesText As String = "", Optional NoText As String = "") As oNode
'construction oNode object
    Dim nd As New oNode
    With nd
        .Name = Name
        .sQuestion = Question
        .ActionNo = ActionNo
        .sAnswer = Answer
        .sTip = Tip
        .NeedAnswer = NeedAnswer
        .YesNode = YesNode
        .NoNode = NoNode
        .PreviousNode = IIf(PreviousNode = "", "", PreviousNode)
        .YesNo = IIf(YesNo = "", "", YesNo)
        .NextNode = IIf(NextNode = "", "", NextNode)
        .YesTextBox = YesTextBox
        .NoTextBox = NoTextBox
        .YesText = YesText
        .NoText = NoText
    End With
    
    Set CreateNode = nd
    ReDim Preserve aryNodes(aryNodeCnt)
    Set aryNodes(aryNodeCnt) = nd
    aryNodeCnt = aryNodeCnt + 1
End Function


Function LoadNode(nodeName As String)
'populate form using object oNode
    Select Case nodeName
'    Case "permitted"
'        MsgBox "Application permitted."
'    Case "exit"
'        MsgBox "Exit."
    Case Else
        Dim nd As New oNode
        Set nd = GetNodeByName(nodeName)
        'put question number before question, except for exit/permitted
        If nd.Name = "exit" Or nd.Name = "permitted" Then
            fmNodes.lbQuestion.Caption = nd.sQuestion
        Else
            fmNodes.lbQuestion.Caption = IIf(ShowQuestionNo, nd.Name & " ", "") & nd.sQuestion '###put node name before question text
        End If
        fmNodes.lbAnswer.Caption = IIf(nd.NeedAnswer, DefaultAnswerText, "") 'nd.sAnswer
        fmNodes.lbAnswer.Enabled = True 'IIf(nd.NeedAnswer, True, False)  'disable textbox if no text answer needed.
        'fmNodes.lbTitle.Enabled = False 'IIf(nd.NeedAnswer, True, False)
        If nd.ActionNo = 0 Then
            fmNodes.fmActions.Enabled = False
            fmNodes.obYes.Enabled = False
            fmNodes.obNo.Enabled = False
            sNextNode = nd.YesNode     'if no choice needed, then link to 'YesNode' by default
            nd.NextNode = nd.YesNode
        Else
            fmNodes.fmActions.Enabled = True
            fmNodes.obYes.Enabled = True
            fmNodes.obNo.Enabled = True
            Select Case nd.YesNo
            Case "y"
                EnableYesNoEvent = False
                fmNodes.obYes.Value = True
                EnableYesNoEvent = True
            Case "n"
                EnableYesNoEvent = False
                fmNodes.obNo.Value = True
                EnableYesNoEvent = True
            Case Else
                EnableYesNoEvent = False
                fmNodes.obYes.Value = False
                fmNodes.obNo.Value = False
                EnableYesNoEvent = True
            End Select
        End If
        'set button status
        fmNodes.btnPrevious.Enabled = IIf(nd.PreviousNode = "", False, True)
        sHelpText = nd.sTip
        fmNodes.btnHelp.Visible = IIf(sHelpText = "", False, True)
        sCurrent = nodeName

        'set text in answer text box
        If nd.sAnswer <> "" And Left(nd.sAnswer, 3) <> "IPP" Then
            fmNodes.lbAnswer.Caption = DefaultAnswerText 'nd.sAnswer
        End If
    End Select
    '###set button capiton
    If nodeName = "exit" Or nodeName = "permitted" Then
        fmNodes.btnNext.Caption = "Finish"
        fmNodes.lbAnswer.Enabled = True
        fmNodes.lbAnswer.Caption = "When you click Finish a Word document will open and you will be able to add to the Documentation output of the decision-making process. You can edit any areas that are shaded pale yellow."
        fmNodes.fmActions.Enabled = False
        fmNodes.obYes.Enabled = False
        fmNodes.obNo.Enabled = False
        If nodeName = "exit" Then
            fmNodes.imgNo.Visible = True
            fmNodes.imgYes.Visible = False
            fmNodes.lbQuestion.Caption = GetNodeByName(nd.PreviousNode).NoText '###
        End If
        If nodeName = "permitted" Then
            fmNodes.imgNo.Visible = False
            fmNodes.imgYes.Visible = True
            fmNodes.lbQuestion.Caption = GetNodeByName(nd.PreviousNode).YesText
        End If
    Else
        fmNodes.btnNext.Caption = "Next"
        fmNodes.imgNo.Visible = False
        fmNodes.imgYes.Visible = False
    End If
    'set pop up form title
    If IsNumeric(nodeName) Then
        If 0 < nodeName < 14 Then
            fmNodes.Caption = "Applying the IPP exceptions - Purpose of disclosure"
        ElseIf 13 < nodeName < 34 Then
            fmNodes.Caption = "Applying the IPP exceptions - Maintenance of the law and related exceptions"
        ElseIf 33 < nodeName < 43 Then
            fmNodes.Caption = "Applying the IPP exceptions - Serious threat"
        ElseIf 50 < nodeName < 57 Then
            fmNodes.Caption = "Legislative vehicles for sharing personal information - Overview"
        End If
    End If

End Function

Function GotAction() As Boolean
'check whether Yes/No option button clicked
    If fmNodes.obNo.Value Or fmNodes.obYes.Value Then
        GotAction = True
    Else
        GotAction = False
    End If
End Function

Function GetNodeByName(s As String) As oNode
    Dim nd As New oNode
    Set GetNodeByName = Nothing
    Dim i As Integer
    For i = 0 To UBound(aryNodes)
        Set nd = aryNodes(i)
        If nd.Name = s Then
            Set GetNodeByName = nd
            Exit Function
        End If
    Next i
End Function

Function GetNodeIndexByName(s As String) As Integer
    GetNodeIndexByName = -1
    For i = 0 To UBound(aryNodes) - 1
        If aryNodes(i).Name = s Then
            GetNodeIndexByName = i
            Exit For
        End If
    Next i
End Function
