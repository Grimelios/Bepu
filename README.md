# Bepu

This is a small project showcasing a simplified version of a physics-related issue I've noticed in my engine. My intent with this project is to determine whether the problem is a logical bug on my end, a misunderstanding of Bepu's functionality, or a bug in the physics engine itself.

**UPDATE:** The problem was thankfully that I misunderstood speculative margin. Using a speculative margin of `float.MaxValue` (the default) avoids the problem entirely, here meaning that narrow-phase callbacks always occur _before_ or _as_ one object penetrates another, not _after_. See https://github.com/bepu/bepuphysics2/discussions/297.
