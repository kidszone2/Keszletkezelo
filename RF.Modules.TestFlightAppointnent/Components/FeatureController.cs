/*
' Copyright (c) 2023 BCE
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using System.Collections.Generic;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for RF.Modules.TestFlightAppointnent
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {


        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<RF.Modules.TestFlightAppointnentInfo> colRF.Modules.TestFlightAppointnents = GetRF.Modules.TestFlightAppointnents(ModuleID);
        //if (colRF.Modules.TestFlightAppointnents.Count != 0)
        //{
        //    strXML += "<RF.Modules.TestFlightAppointnents>";

        //    foreach (RF.Modules.TestFlightAppointnentInfo objRF.Modules.TestFlightAppointnent in colRF.Modules.TestFlightAppointnents)
        //    {
        //        strXML += "<RF.Modules.TestFlightAppointnent>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objRF.Modules.TestFlightAppointnent.Content) + "</content>";
        //        strXML += "</RF.Modules.TestFlightAppointnent>";
        //    }
        //    strXML += "</RF.Modules.TestFlightAppointnents>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlRF.Modules.TestFlightAppointnents = DotNetNuke.Common.Globals.GetContent(Content, "RF.Modules.TestFlightAppointnents");
        //foreach (XmlNode xmlRF.Modules.TestFlightAppointnent in xmlRF.Modules.TestFlightAppointnents.SelectNodes("RF.Modules.TestFlightAppointnent"))
        //{
        //    RF.Modules.TestFlightAppointnentInfo objRF.Modules.TestFlightAppointnent = new RF.Modules.TestFlightAppointnentInfo();
        //    objRF.Modules.TestFlightAppointnent.ModuleId = ModuleID;
        //    objRF.Modules.TestFlightAppointnent.Content = xmlRF.Modules.TestFlightAppointnent.SelectSingleNode("content").InnerText;
        //    objRF.Modules.TestFlightAppointnent.CreatedByUser = UserID;
        //    AddRF.Modules.TestFlightAppointnent(objRF.Modules.TestFlightAppointnent);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<RF.Modules.TestFlightAppointnentInfo> colRF.Modules.TestFlightAppointnents = GetRF.Modules.TestFlightAppointnents(ModInfo.ModuleID);

        //foreach (RF.Modules.TestFlightAppointnentInfo objRF.Modules.TestFlightAppointnent in colRF.Modules.TestFlightAppointnents)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objRF.Modules.TestFlightAppointnent.Content, objRF.Modules.TestFlightAppointnent.CreatedByUser, objRF.Modules.TestFlightAppointnent.CreatedDate, ModInfo.ModuleID, objRF.Modules.TestFlightAppointnent.ItemId.ToString(), objRF.Modules.TestFlightAppointnent.Content, "ItemId=" + objRF.Modules.TestFlightAppointnent.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
