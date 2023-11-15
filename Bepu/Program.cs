using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuUtilities.Memory;

namespace Bepu
{
	internal sealed class Program
	{
		public static void Main(string[] args)
		{
			const float SpeculativeMargin = 0.1f;
			const float SphereRadius = 1.0f;

			// Create the simulation.
			var bufferPool = new BufferPool();
			var narrowPhaseCallbacks = new NarrowPhaseCallbacks();
			var poseIntegratorCallbacks = new PoseIntegratorCallbacks();
			var solveDescription = new SolveDescription(1, 1);
			var timestepper = new DefaultTimestepper();
			var simulation = Simulation.Create(bufferPool, narrowPhaseCallbacks, poseIntegratorCallbacks,
				solveDescription, timestepper);

			// Create a static box. The box's dimensions and position result in a "ground" position at Y = 0.
			var box = new Box(6, 6, 6);
			var boxShape = simulation.Shapes.Add(box);
			var boxPosition = new Vector3(0, -3, 0);
			var boxOrientation = Quaternion.Identity;
			var boxDescription = new StaticDescription(boxPosition, boxOrientation, boxShape);
			var boxHandle = simulation.Statics.Add(boxDescription);

			// Create a dynamic sphere. Since the ground position is at Y = 0, I'd expect a contact to occur (i.e. the
			// narrow-phase callbacks to be called) when the sphere's Y coordinate is less than or equal to its radius
			// plus the speculative margin.
			var sphere = new Sphere(SphereRadius);
			var sphereInertia = box.ComputeInertia(1);
			var sphereShape = simulation.Shapes.Add(sphere);
			var spherePosition = new Vector3(0, 10, 0);
			var sphereOrientation = Quaternion.Identity;
			var spherePose = new RigidPose(spherePosition, sphereOrientation);
			var sphereVelocity = new BodyVelocity();
			var sphereCollidableDescription = new CollidableDescription(sphereShape, SpeculativeMargin);
			var sphereActivityDescription = new BodyActivityDescription(0.01f);
			var sphereBodyDescription = BodyDescription.CreateDynamic(spherePose, sphereVelocity, sphereInertia, 
				sphereCollidableDescription, sphereActivityDescription);
			var sphereHandle = simulation.Bodies.Add(sphereBodyDescription);

			// Infinitely step the simulation, simulating a 60 FPS game.
			var penetrationDetected = false;

			while (true)
			{
				simulation.Timestep(1f / 60);

				var sphereReference = simulation.Bodies.GetBodyReference(sphereHandle);
				var sphereVelocityY = sphereReference.Velocity.Linear.Y;
				var spherePositionY = sphereReference.Pose.Position.Y;
				var narrowPhaseCalled = ((NarrowPhase<NarrowPhaseCallbacks>)simulation.NarrowPhase).Callbacks.Called;

				if (penetrationDetected)
				{
					// If both relevant scenarios have been detected, the infinite loop can safely be broken.
					if (narrowPhaseCalled)
					{
						break;
					}
				}
				else if (spherePositionY <= SphereRadius + SpeculativeMargin && sphereVelocityY < 0 && 
					!narrowPhaseCalled)
				{
					Console.WriteLine("Sphere penetrated box with downward velocity (narrow phase NOT yet called).");

					penetrationDetected = true;
				}
			}

			Console.WriteLine("Done.");
		}
	}
}
