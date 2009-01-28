﻿using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace PoshWpf
{
   [Cmdlet("Invoke", "BootsWindow", SupportsShouldProcess = false, ConfirmImpact = ConfirmImpact.None, DefaultParameterSetName = "ByIndex")]
   public class InvokeBootsWindowCommand : PSCmdlet
   {
      [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ByIndex")]
      public int[] Index { get; set; }

      [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ByTitle")]
      public string[] Name { get; set; }

      [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ByWindow")]
      public Window[] Window { get; set; }

      [Parameter(Position = 1, Mandatory = true)]
      public ScriptBlock Script { get; set; }

      private List<WildcardPattern> patterns;
      protected override void BeginProcessing()
      {
         if (ParameterSetName == "ByTitle")
         {
            patterns = new List<WildcardPattern>(Name.Length);
            foreach (var title in Name)
            {
               patterns.Add(new WildcardPattern(title));
            }
         }
         base.BeginProcessing();
      }

      protected override void ProcessRecord()
      {
         var windows = SessionState.PSVariable.Get("BootsWindows");

         if (windows != null && windows.Value != null && (windows.Value is BootsWindowDictionary))
         {
            switch (ParameterSetName)
	         {
               case "ByIndex":
                  foreach (var i in Index)
                  {
                     WriteObject(((BootsWindowDictionary)windows.Value)[i].Dispatcher.Invoke(((Func<Collection<PSObject>>)Invoker)));
                  } break;
               case "ByTitle":
                  foreach (var window in ((BootsWindowDictionary)windows.Value).Values)
                  {
                     foreach (var title in patterns)
	                  {
                        if(title.IsMatch( window.Title )) {
                           WriteObject(window.Dispatcher.Invoke(((Func<Collection<PSObject>>)Invoker)));
                        }
                     }
                  } break;
         		case "ByWindow":
                  foreach (var window in Window)
                  {
                     WriteObject(window.Dispatcher.Invoke(((Func<Collection<PSObject>>)Invoker)));
                  } break;
	         }

            if(_error != null) {
               WriteError(_error);
            }
         }

         base.ProcessRecord();
      }

      ErrorRecord _error = null;
      private Collection<PSObject> Invoker()
      {
         Collection<PSObject> result = null;
         try
         {
            result = Script.Invoke();
         }
         catch (Exception ex)
         {
            _error = new ErrorRecord(ex, "Error during invoke", ErrorCategory.OperationStopped, Script);
         }
         return result;
      }

   }
}