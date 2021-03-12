using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//TODO: fix paging
public class DialogUI : MonoBehaviour
{
    [SerializeField] Text speakerTextField;
    [SerializeField] Text messageTextField;
    [SerializeField] TextMeshProUGUI[] responseRows;
    [SerializeField] Color currentColor;

    public UnityEvent<string> onOptionSelected;

    Color defaultColor = Color.white;
    Image dialogBackground;

    public List<ResponseItem> responses;
    int responsePageIndex;
    bool isWaitingForInput;

    public struct ResponseItem
    {
        public bool isNextPage;
        public string uid;
        public string text;
    }

    private void Awake()
    {
        dialogBackground = GetComponent<Image>();

        if (responseRows == null)
            Debug.LogError("responses rows not set");

        if (onOptionSelected == null)
            onOptionSelected = new UnityEvent<string>();
    }

    private void Update()
    {
        //if (isWaitingForInput)
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        SelectOption(0);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //    {
        //        SelectOption(1);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha3))
        //    {
        //        SelectOption(2);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Alpha4))
        //    {
        //        SelectOption(3);
        //    }
        //}
    }
    public void ResetDialog()
    {
        this.DisplayMessage(string.Empty, string.Empty, defaultColor);
        speakerTextField.text = string.Empty;
        dialogBackground.enabled = false;
    }
    public void DisplayMessage(string speaker, string message, Color color)
    {
        HideResponses();
        dialogBackground.enabled = true;
        speakerTextField.text = speaker + ":";
        speakerTextField.color = color;
        messageTextField.text = message;
        messageTextField.color = color;
    }
    public void EndConversation()
    {
        ResetDialog();
        dialogBackground.enabled = false;
    }

    public void BindResponses(Dictionary<string, string> availableResponses)
    {
        if (availableResponses != null)
        {
            responses = new List<ResponseItem>();
            if (availableResponses.Count > responseRows.Length)
            {
                //Need paging
                int index = 0;
                foreach (var key in availableResponses.Keys)
                {
                    if (index % (responseRows.Length - 1) == 0)
                    {
                        //if last item on page
                        responses.Add(new ResponseItem { uid = null, text = "[more]", isNextPage = true });
                    }
                    else
                        responses.Add(new ResponseItem { uid = key, text = $"{index + 1}. {availableResponses[key]}" });
                    index++;
                }
            }
            else
            {
                //Only one page
                foreach (var key in availableResponses.Keys)
                    responses.Add(new ResponseItem { uid = key, text = availableResponses[key] });
            }
            responsePageIndex = 0;
        }
        DrawResponses();
        isWaitingForInput = true;

        if (!responses.Any())
            Debug.LogError("No responses bound");
    }

    public void NextPage()
    {
        ++responsePageIndex;
        DrawResponses();
    }

    void DrawResponses()
    {
        for (int i = 0; i < responseRows.Length; ++i)
        {
            var responseItemIndex = i + responsePageIndex * responseRows.Length;
            if (responseItemIndex < responses.Count)
            {
                responseRows[i].gameObject.SetActive(true);
                responseRows[i].text = responses[responseItemIndex].text;
            }
            else
                responseRows[i].gameObject.SetActive(false);
        }
    }

    void HideResponses()
    {
        responses?.Clear();
        for (int i = 0; i < responseRows.Length; ++i)
        {
            responseRows[i].gameObject.SetActive(false);
            responseRows[i].text = string.Empty;
        }
    }

    public void SelectOption(int rowIndex)
    {
        var response = responses[rowIndex + responsePageIndex * responseRows.Length];
        onOptionSelected?.Invoke(response.uid);

        //TODO:fix paging
        //if (response.isNextPage)
        //{
        //    NextPage();
        //}
    }
}
