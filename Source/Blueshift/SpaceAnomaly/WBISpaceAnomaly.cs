﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;
using KSP.UI.Screens;
using KSP.Localization;

/*
Source code copyrighgt 2015, by Michael Billard (Angel-125)
License: GPLV3

If you want to use this code, give me a shout on the KSP forums! :)
Wild Blue Industries is trademarked by Michael Billard and may be used for non-commercial purposes. All other rights reserved.
Note that Wild Blue Industries is a ficticious entity 
created for entertainment purposes. It is in no way meant to represent a real entity.
Any similarity to a real entity is purely coincidental.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace Blueshift
{
    /// <summary>
    /// Describes a space anomaly. Similar to asteroids, space anomalies are listed as unknown objects until tracked and visited. Each type of anomaly is defined by a SPACE_ANOMALY config node.
    /// </summary>
    public class WBISpaceAnomaly
    {
        #region Constants
        public const string kNodeName = "SPACE_ANOMALY";
        public const string kName = "name";
        public const string kPartName = "partName";
        public const string kSpawnMode = "spawnMode";
        public const string kFixedBody = "fixedBody";
        public const string kFixedSMA = "fixedSMA";
        public const string kFixedEccentricity = "fixedEccentricity";
        public const string kFixedInclination = "fixedInclination";
        public const string kMinLifetime = "minLifetime";
        public const string kMaxLifetime = "maxLifetime";
        public const string kSpawnTargetNumber = "spawnTargetNumber";
        public const string kMaxInstances = "maxInstances";
        public const string kVesselId = "vesselId";
        public const string kSizeClass = "sizeClass";
        public const string kOrbitType = "orbitType";
        public const string kFlyByOrbitChance = "flyByOrbitChance";
        public const string kMaxDaysToClosestApproach = "maxDaysToClosestApproach";
        #endregion

        #region Fields
        /// <summary>
        /// Identifier for the space anomaly.
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// Name of the part to spawn
        /// </summary>
        public string partName = string.Empty;

        /// <summary>
        /// Like asteroids, space anomalies have a size class that ranges from Size A (12 meters) to Size I (100+ meters).
        /// The default is A.
        /// </summary>
        public string sizeClass = "A";

        /// <summary>
        /// How does an instance spawn
        /// </summary>
        public WBIAnomalySpawnModes spawnMode = WBIAnomalySpawnModes.randomOrbit;

        /// <summary>
        /// The type of orbit to create. Default is elliptical.
        /// </summary>
        public WBIAnomalyOrbitTypes orbitType = WBIAnomalyOrbitTypes.elliptical;

        /// <summary>
        /// For flyBy orbits, the max number of days until the anomaly reaches the closest point in its orbit. Default is 30.
        /// </summary>
        public float maxDaysToClosestApproach = 30;

        /// <summary>
        /// For orbitType = random, on a roll of 1 to 100, what is the chance that the orbit will be flyBy. Default is 50.
        /// </summary>
        public int flyByOrbitChance = 50;

        /// <summary>
        /// For fixedOrbit, the celestial body to spawn around.
        /// </summary>
        public string fixedBody = string.Empty;

        /// <summary>
        /// For fixedOrbit, the Semi-Major axis of the orbit.
        /// </summary>
        public double fixedSMA = 0;

        /// <summary>
        /// For fixedOrbit, the eccentrcity of the orbit.
        /// </summary>
        public double fixedEccentricity = 0;

        /// <summary>
        /// Fixed inclination. Only used for fixedOrbit. If set to -1 then a random inclination will be used instead.
        /// </summary>
        public double fixedInclination = -1f;

        /// <summary>
        /// For undiscovered objects, the minimum number of seconds that the anomaly can exist. Default is 86400 (1 day).
        /// Set to -1 to use maximum possible value. When set to -1, maxLifetime is ignored.
        /// </summary>
        public double minLifetime = 86400f;

        /// <summary>
        /// For undiscovered objects, the maximum number of seconds that the anomaly can exist. Default is 1728000 (20 days).
        /// </summary>
        public double maxLifetime = 1728000;

        /// <summary>
        /// Spawn chance in a roll between 1 and 1000
        /// </summary>
        public int spawnTargetNumber = 995;

        /// <summary>
        /// Maximum number of objects of this type that may exist at any given time. Default is 10.
        /// Set to -1 for unlimited number.
        /// </summary>
        public int maxInstances = 10;

        /// <summary>
        /// ID of the vessel as found in the FlightGlobals.VesselsUnloaded.
        /// </summary>
        public string vesselId = string.Empty;
        #endregion

        #region Housekeeping
        int lastSeed = 0;
        #endregion

        #region Initializers
        public static WBISpaceAnomaly CreateFromNode(ConfigNode node)
        {
            WBISpaceAnomaly anomaly = new WBISpaceAnomaly();

            if (node.HasValue(kName))
                anomaly.name = node.GetValue(kName);

            if (node.HasValue(kPartName))
                anomaly.partName = node.GetValue(kPartName);

            if (node.HasValue(kSizeClass))
                anomaly.sizeClass = node.GetValue(kSizeClass);

            if (node.HasValue(kSpawnMode))
                anomaly.spawnMode = (WBIAnomalySpawnModes)Enum.Parse(typeof(WBIAnomalySpawnModes), node.GetValue(kSpawnMode));

            if (node.HasValue(kFixedBody))
                anomaly.fixedBody = node.GetValue(kFixedBody);

            if (node.HasValue(kFixedSMA))
                double.TryParse(node.GetValue(kFixedSMA), out anomaly.fixedSMA);

            if (node.HasValue(kFixedEccentricity))
                double.TryParse(node.GetValue(kFixedEccentricity), out anomaly.fixedEccentricity);

            if (node.HasValue(kFixedInclination))
                double.TryParse(node.GetValue(kFixedInclination), out anomaly.fixedInclination);

            if (node.HasValue(kMinLifetime))
                double.TryParse(node.GetValue(kMinLifetime), out anomaly.minLifetime);

            if (node.HasValue(kMaxLifetime))
                double.TryParse(node.GetValue(kMaxLifetime), out anomaly.maxLifetime);

            if (node.HasValue(kSpawnTargetNumber))
                int.TryParse(node.GetValue(kSpawnTargetNumber), out anomaly.spawnTargetNumber);

            if (node.HasValue(kMaxInstances))
                int.TryParse(node.GetValue(kMaxInstances), out anomaly.maxInstances);

            if (node.HasValue(kVesselId))
                anomaly.vesselId = node.GetValue(kVesselId);

            if (node.HasValue(kOrbitType))
                Enum.TryParse(node.GetValue(kOrbitType), out anomaly.orbitType);

            if (node.HasValue(kFlyByOrbitChance))
                int.TryParse(node.GetValue(kFlyByOrbitChance), out anomaly.flyByOrbitChance);

            if (node.HasValue(kMaxDaysToClosestApproach))
                float.TryParse(node.GetValue(kMaxDaysToClosestApproach), out anomaly.maxDaysToClosestApproach);

            return anomaly;
        }

        public WBISpaceAnomaly()
        {

        }

        public WBISpaceAnomaly(WBISpaceAnomaly copyFrom)
        {
            name = copyFrom.name;
            partName = copyFrom.partName;
            sizeClass = copyFrom.sizeClass;
            spawnMode = copyFrom.spawnMode;
            orbitType = copyFrom.orbitType;
            maxDaysToClosestApproach = copyFrom.maxDaysToClosestApproach;
            flyByOrbitChance = copyFrom.flyByOrbitChance;
            fixedBody = copyFrom.fixedBody;
            fixedSMA = copyFrom.fixedSMA;
            fixedEccentricity = copyFrom.fixedEccentricity;
            fixedInclination = copyFrom.fixedInclination;
            minLifetime = copyFrom.minLifetime;
            maxLifetime = copyFrom.maxLifetime;
            spawnTargetNumber = copyFrom.spawnTargetNumber;
            maxInstances = copyFrom.maxInstances;
            vesselId = copyFrom.vesselId;

            lastSeed = UnityEngine.Random.Range(0, int.MaxValue);
            UnityEngine.Random.InitState(lastSeed);
        }

        public ConfigNode Save()
        {
            ConfigNode node = new ConfigNode(kNodeName);

            node.AddValue(kName, name);
            node.AddValue(kPartName, partName);
            node.AddValue(kSizeClass, sizeClass);
            node.AddValue(kSpawnMode, spawnMode.ToString());
            node.AddValue(kOrbitType, orbitType.ToString());
            node.AddValue(kMaxDaysToClosestApproach, maxDaysToClosestApproach.ToString());
            node.AddValue(kFlyByOrbitChance, flyByOrbitChance.ToString());
            node.AddValue(kFixedBody, fixedBody);
            node.AddValue(kFixedSMA, fixedSMA.ToString());
            node.AddValue(kFixedEccentricity, fixedEccentricity.ToString());
            node.AddValue(kFixedInclination, fixedInclination.ToString());
            node.AddValue(kMinLifetime, minLifetime);
            node.AddValue(kMaxLifetime, maxLifetime);
            node.AddValue(kSpawnTargetNumber, spawnTargetNumber.ToString());
            node.AddValue(kMaxInstances, maxInstances.ToString());
            node.AddValue(kVesselId, vesselId);

            return node;
        }
        #endregion

        #region API
        /// <summary>
        /// Checks to see if we should create a new instance.
        /// </summary>
        public void CreateNewInstancesIfNeeded(List<WBISpaceAnomaly> spaceAnomalies)
        {
            // Roll RNG to make sure we're allowed to spawn a new anomaly.
            if (UnityEngine.Random.Range(1, 1000) < spawnTargetNumber)
                return;

            // Make sure that we can create at least one new instance.
            if (!canCreateNewInstance(spaceAnomalies))
                return;

            if (spawnMode != WBIAnomalySpawnModes.everyLastPlanet)
            {
                spaceAnomalies.Add(createRandomAnomaly());
            }
            else
            {
                spaceAnomalies.AddRange(createLastPlanetAnomalies(spaceAnomalies));
            }
        }
        #endregion

        #region Helpers
        private WBISpaceAnomaly createRandomAnomaly()
        {
            WBISpaceAnomaly anomaly = new WBISpaceAnomaly(this);
            ConfigNode vesselNode = createAnomalyVessel(anomaly);

            return anomaly;
        }

        private List<WBISpaceAnomaly> createLastPlanetAnomalies(List<WBISpaceAnomaly> existingAnomalies)
        {
            // Filter the existing anomalies based on our parameters.
            List<WBISpaceAnomaly> filteredAnomalies = existingAnomalies.FindAll(p => p.spawnMode == spawnMode && p.partName == partName);

            List<WBISpaceAnomaly> anomalies = new List<WBISpaceAnomaly>();
            WBISpaceAnomaly anomaly;
            ConfigNode vesselNode;

            // Get the list of last planets
            List<CelestialBody> lastPlanets = BlueshiftScenario.shared.GetEveryLastPlanet();

            // Build the string containing the names of the bodies with anomalies.
            StringBuilder stringBuilder = new StringBuilder();
            int count = filteredAnomalies.Count;
            for (int index = 0; index < count; index++)
                stringBuilder.Append(filteredAnomalies[index].fixedBody);
            string bodiesWithAnomalies = stringBuilder.ToString();

            // Now check the list of last planets and see if the name of the body is in the string.
            count = lastPlanets.Count;
            for (int index = 0; index < count; index++)
            {
                if (!bodiesWithAnomalies.Contains(lastPlanets[index].name))
                {
                    anomaly = new WBISpaceAnomaly(this);
                    anomaly.fixedBody = lastPlanets[index].name;
                    vesselNode = createAnomalyVessel(anomaly);
                    anomalies.Add(anomaly);
                }
            }

            return anomalies;
        }

        private ConfigNode createAnomalyVessel(WBISpaceAnomaly anomaly)
        {
            // Generate vessel name
            string vesselName = DiscoverableObjectsUtil.GenerateAsteroidName();
            string prefix = Localizer.Format("#autoLOC_6001923");
            prefix = prefix.Replace(" <<1>>", "");
            vesselName = vesselName.Replace(prefix, "UNK-");
            vesselName = vesselName.Replace("- ", "-");

            // Generate orbit
            Orbit orbit = generateOrbit(anomaly);

            // Create part nodes
            ConfigNode[] partNodes = new ConfigNode[] { ProtoVessel.CreatePartNode(anomaly.partName, 0) };

            // Determine lifetime
            double minLifetime = anomaly.minLifetime;
            double maxLifetime = anomaly.maxLifetime;
            if (minLifetime < 0)
            {
                minLifetime = double.MaxValue;
                maxLifetime = double.MaxValue;
            }

            // Setup object class
            UntrackedObjectClass objectClass = UntrackedObjectClass.A;
            Enum.TryParse(anomaly.sizeClass, out objectClass);

            // Create discovery and additional nodes.
            ConfigNode discoveryNode = ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Presence, objectClass, minLifetime, maxLifetime);
            ConfigNode[] additionalNodes = new ConfigNode[] { new ConfigNode("ACTIONGROUPS"), discoveryNode };

            // Create vessel node
            ConfigNode vesselNode = ProtoVessel.CreateVesselNode(vesselName, VesselType.SpaceObject, orbit, 0, partNodes, additionalNodes);

            // Add vessel node to the game.
            HighLogic.CurrentGame.AddVessel(vesselNode);

            // Get vessel ID
            if (vesselNode.HasValue("persistentId"))
                anomaly.vesselId = vesselNode.GetValue("persistentId");

            return vesselNode;
        }

        private Orbit generateOrbit(WBISpaceAnomaly anomaly)
        {
            Orbit orbit = null;
            int bodyIndex = 0;
            CelestialBody body = null;

            switch (anomaly.spawnMode)
            {
                case WBIAnomalySpawnModes.fixedOrbit:
                    body = FlightGlobals.GetBodyByName(anomaly.fixedBody);
                    if (body != null)
                    {
                        double inclination = anomaly.fixedInclination;
                        if (inclination < 0)
                            inclination = UnityEngine.Random.Range(0, 90);
                        return new Orbit(inclination, anomaly.fixedEccentricity, anomaly.fixedSMA, 0, 0, 0, Planetarium.GetUniversalTime(), body);
                    }
                    break;

                case WBIAnomalySpawnModes.randomOrbit:
                    List<CelestialBody> bodies = FlightGlobals.fetch.bodies;
                    bodyIndex = UnityEngine.Random.Range(0, bodies.Count - 1);
                    body = bodies[bodyIndex];
                    break;

                case WBIAnomalySpawnModes.randomSolarOrbit:
                    List<CelestialBody> stars = BlueshiftScenario.shared.GetStars();
                    bodyIndex = UnityEngine.Random.Range(0, stars.Count - 1);
                    body = stars[bodyIndex];
                    break;

                case WBIAnomalySpawnModes.randomPlanetOrbit:
                    List<CelestialBody> planets = BlueshiftScenario.shared.GetPlanets();
                    bodyIndex = UnityEngine.Random.Range(0, planets.Count - 1);
                    body = planets[bodyIndex];
                    break;

                case WBIAnomalySpawnModes.everyLastPlanet:
                    body = FlightGlobals.GetBodyByName(anomaly.fixedBody);
                    break;
            }

            if (body != null)
            {
                switch (orbitType)
                {
                    case WBIAnomalyOrbitTypes.elliptical:
                        orbit = Orbit.CreateRandomOrbitAround(body);
                        break;

                    case WBIAnomalyOrbitTypes.flyBy:
                        orbit = Orbit.CreateRandomOrbitFlyBy(body, UnityEngine.Random.Range(0, anomaly.maxDaysToClosestApproach));
                        break;

                    case WBIAnomalyOrbitTypes.random:
                        if (UnityEngine.Random.Range(1, 100) >= anomaly.flyByOrbitChance)
                            orbit = Orbit.CreateRandomOrbitFlyBy(body, UnityEngine.Random.Range(0, anomaly.maxDaysToClosestApproach));
                        else
                            orbit = Orbit.CreateRandomOrbitAround(body);
                        break;
                }
            }

            return orbit;
        }

        private bool canCreateNewInstance(List<WBISpaceAnomaly> existingAnomalies)
        {
            // Filter the existing anomalies based on our parameters.
            List<WBISpaceAnomaly> filteredAnomalies = existingAnomalies.FindAll(p => p.spawnMode == spawnMode && p.partName == partName);

            // Check for fixed orbit.
            if (spawnMode == WBIAnomalySpawnModes.fixedOrbit)
            {
                if (existingAnomalies.Find(p => p.fixedBody == fixedBody) != null)
                    return false;
            }

            // Check for everyLastPlanet.
            else if (spawnMode == WBIAnomalySpawnModes.everyLastPlanet)
            {
                // Get the list of last planets
                List<CelestialBody> lastPlanets = BlueshiftScenario.shared.GetEveryLastPlanet();

                // Build the string containing the names of the bodies with anomalies.
                StringBuilder stringBuilder = new StringBuilder();
                int count = filteredAnomalies.Count;
                for (int index = 0; index < count; index++)
                    stringBuilder.Append(filteredAnomalies[index].fixedBody);
                string bodiesWithAnomalies = stringBuilder.ToString();

                // Now check the list of last planets and see if the name of the body is in the string.
                count = lastPlanets.Count;
                for (int index = 0; index < count; index++)
                {
                    if (!bodiesWithAnomalies.Contains(lastPlanets[index].name))
                        return true;
                }
            }

            // Check for max instances
            else if (maxInstances > 0 && filteredAnomalies.Count >= maxInstances)
                return false;

            // We can create a new instance.
            return true;
        }
        #endregion
    }
}
