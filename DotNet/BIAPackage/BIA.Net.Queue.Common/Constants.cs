// <copyright file="Constants.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Static class with all constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Get Default value of queue's name for file Receiver.
        /// </summary>
        public static string DefaultFileReceiverQueueName = "Bia_GenericTransfertAgent_Queue";

        /// <summary>
        /// Get Default value of number of retry.
        /// </summary>
        public static int DefaultNbRetry = 3;

        /// <summary>
        /// Get Default value of keeping files in folders in Month(s).
        /// </summary>
        public static int DefaultMonthOfKeepingFileInFolders = 3;

        /// <summary>
        /// Get Default value of wait time beetween try in seconde(s).
        /// </summary>
        public static int DefaultBaseWaitTimeInSec = 5;
    }
}
