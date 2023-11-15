using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;

namespace Bepu
{
	internal struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
	{
		public bool Called { get; private set; }

		readonly bool INarrowPhaseCallbacks.AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA,
			int childIndexB)
		{
			return true;
		}

		readonly bool INarrowPhaseCallbacks.ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair,
			ref TManifold manifold, out PairMaterialProperties pairMaterial)
		{
			pairMaterial = default;

			return true;
		}

		readonly bool INarrowPhaseCallbacks.ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA,
			int childIndexB, ref ConvexContactManifold manifold)
		{
			return true;
		}

		readonly void INarrowPhaseCallbacks.Dispose()
		{
		}

		readonly void INarrowPhaseCallbacks.Initialize(Simulation simulation)
		{
		}
		
		bool INarrowPhaseCallbacks.AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b,
			ref float speculativeMargin)
		{
			if (!Called)
			{
				Console.WriteLine("Narrow phase called.");

				Called = true;
			}

			return true;
		}
	}
}
