using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics;
using BepuUtilities;

namespace Bepu
{
	internal readonly struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
	{
		private readonly Vector<float> gravity;

		public PoseIntegratorCallbacks()
		{
			const float GravityMagnitude = 9.81f;

			gravity = new Vector<float>(-GravityMagnitude);
		}

		AngularIntegrationMode IPoseIntegratorCallbacks.AngularIntegrationMode => AngularIntegrationMode.Nonconserving;
		bool IPoseIntegratorCallbacks.AllowSubstepsForUnconstrainedBodies => true;
		bool IPoseIntegratorCallbacks.IntegrateVelocityForKinematics => true;

		void IPoseIntegratorCallbacks.Initialize(Simulation simulation)
		{
		}

		void IPoseIntegratorCallbacks.IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, 
			QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, 
			Vector<float> dt, ref BodyVelocityWide velocity)
		{
			velocity.Linear.Y += gravity * dt;
		}

		void IPoseIntegratorCallbacks.PrepareForIntegration(float dt)
		{
		}
	}
}
