/*******************************************************************************
* Copyright 2011 badlogicgames, vesuvio, xoppa.
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
*   http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsPV
{
    /** Encapsulates a 3D vector. Allows chaining operations by returning a reference to itself in all modification methods.
     * @author badlogicgames@gmail.com */
    public class Vector3GDX
    {
        public const float DegreesToRadians = (float)(Math.PI / 180), RadiansToDegrees = (float)(180 / Math.PI);

        /** the x-component of this vector **/
        public float x;
        /** the y-component of this vector **/
        public float y;
        /** the z-component of this vector **/
        public float z;

        public static Vector3GDX X = new Vector3GDX(1, 0, 0);
        public static Vector3GDX Y = new Vector3GDX(0, 1, 0);
        public static Vector3GDX Z = new Vector3GDX(0, 0, 1);
        public static Vector3GDX Zero = new Vector3GDX(0, 0, 0);

        /** Constructs a vector at (0,0,0) */
        public Vector3GDX()
        {
        }

        /** Creates a vector with the given components
         * @param x The x-component
         * @param y The y-component
         * @param z The z-component */
        public Vector3GDX(float x, float y, float z)
        {
            this.set(x, y, z);
        }

        /** Creates a vector from the given vector
         * @param vector The vector */
        public Vector3GDX(Vector3GDX vector)
        {
            this.set(vector);
        }

        /** Creates a vector from the given array. The array must have at least 3 elements.
         *
         * @param values The array */
        public Vector3GDX(float[] values)
        {
            this.set(values[0], values[1], values[2]);
        }

        /** Sets the vector to the given components
         *
         * @param x The x-component
         * @param y The y-component
         * @param z The z-component
         * @return this vector for chaining */
        public Vector3GDX set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }


        public Vector3GDX set(Vector3GDX vector)
        {
            return this.set(vector.x, vector.y, vector.z);
        }

        /** Sets the components from the array. The array must have at least 3 elements
         *
         * @param values The array
         * @return this vector for chaining */
        public Vector3GDX set(float[] values)
        {
            return this.set(values[0], values[1], values[2]);
        }


        public Vector3GDX cpy()
        {
            return new Vector3GDX(this);
        }


        public Vector3GDX add(Vector3GDX vector)
        {
            return this.add(vector.x, vector.y, vector.z);
        }

        /** Adds the given vector to this component
         * @param x The x-component of the other vector
         * @param y The y-component of the other vector
         * @param z The z-component of the other vector
         * @return This vector for chaining. */
        public Vector3GDX add(float x, float y, float z)
        {
            return this.set(this.x + x, this.y + y, this.z + z);
        }

        /** Adds the given value to all three components of the vector.
         *
         * @param values The value
         * @return This vector for chaining */
        public Vector3GDX add(float values)
        {
            return this.set(this.x + values, this.y + values, this.z + values);
        }


        public Vector3GDX sub(Vector3GDX a_vec)
        {
            return this.sub(a_vec.x, a_vec.y, a_vec.z);
        }

        /** Subtracts the other vector from this vector.
         *
         * @param x The x-component of the other vector
         * @param y The y-component of the other vector
         * @param z The z-component of the other vector
         * @return This vector for chaining */
        public Vector3GDX sub(float x, float y, float z)
        {
            return this.set(this.x - x, this.y - y, this.z - z);
        }

        /** Subtracts the given value from all components of this vector
         *
         * @param value The value
         * @return This vector for chaining */
        public Vector3GDX sub(float value)
        {
            return this.set(this.x - value, this.y - value, this.z - value);
        }


        public Vector3GDX scl(float scalar)
        {
            return this.set(this.x * scalar, this.y * scalar, this.z * scalar);
        }


        public Vector3GDX scl(Vector3GDX other)
        {
            return this.set(x * other.x, y * other.y, z * other.z);
        }

        /** Scales this vector by the given values
         * @param vx X value
         * @param vy Y value
         * @param vz Z value
         * @return This vector for chaining */
        public Vector3GDX scl(float vx, float vy, float vz)
        {
            return this.set(this.x * vx, this.y * vy, this.z * vz);
        }


        public Vector3GDX mulAdd(Vector3GDX vec, float scalar)
        {
            this.x += vec.x * scalar;
            this.y += vec.y * scalar;
            this.z += vec.z * scalar;
            return this;
        }


        public Vector3GDX mulAdd(Vector3GDX vec, Vector3GDX mulVec)
        {
            this.x += vec.x * mulVec.x;
            this.y += vec.y * mulVec.y;
            this.z += vec.z * mulVec.z;
            return this;
        }

        /** @return The euclidean length */
        public static float len(float x, float y, float z)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }


        public float len()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /** @return The squared euclidean length */
        public static float len2(float x, float y, float z)
        {
            return x * x + y * y + z * z;
        }


        public float len2()
        {
            return x * x + y * y + z * z;
        }

        /** @param vector The other vector
         * @return Whether this and the other vector are equal */
        public bool idt(Vector3GDX vector)
        {
            return x == vector.x && y == vector.y && z == vector.z;
        }

        /** @return The euclidean distance between the two specified vectors */
        public static float dst(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float a = x2 - x1;
            float b = y2 - y1;
            float c = z2 - z1;
            return (float)Math.Sqrt(a * a + b * b + c * c);
        }


        public float dst(Vector3GDX vector)
        {
            float a = vector.x - x;
            float b = vector.y - y;
            float c = vector.z - z;
            return (float)Math.Sqrt(a * a + b * b + c * c);
        }

        /** @return the distance between this point and the given point */
        public float dst(float x, float y, float z)
        {
            float a = x - this.x;
            float b = y - this.y;
            float c = z - this.z;
            return (float)Math.Sqrt(a * a + b * b + c * c);
        }

        /** @return the squared distance between the given points */
        public static float dst2(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float a = x2 - x1;
            float b = y2 - y1;
            float c = z2 - z1;
            return a * a + b * b + c * c;
        }


        public float dst2(Vector3GDX point)
        {
            float a = point.x - x;
            float b = point.y - y;
            float c = point.z - z;
            return a * a + b * b + c * c;
        }

        /** Returns the squared distance between this point and the given point
         * @param x The x-component of the other point
         * @param y The y-component of the other point
         * @param z The z-component of the other point
         * @return The squared distance */
        public float dst2(float x, float y, float z)
        {
            float a = x - this.x;
            float b = y - this.y;
            float c = z - this.z;
            return a * a + b * b + c * c;
        }


        public Vector3GDX nor()
        {
            float len2 = this.len2();
            if(len2 == 0f || len2 == 1f) return this;
            return this.scl(1f / (float)Math.Sqrt(len2));
        }

        /** @return The dot product between the two vectors */
        public static float dot(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            return x1 * x2 + y1 * y2 + z1 * z2;
        }


        public float dot(Vector3GDX vector)
        {
            return x * vector.x + y * vector.y + z * vector.z;
        }

        /** Returns the dot product between this and the given vector.
         * @param x The x-component of the other vector
         * @param y The y-component of the other vector
         * @param z The z-component of the other vector
         * @return The dot product */
        public float dot(float x, float y, float z)
        {
            return this.x * x + this.y * y + this.z * z;
        }

        /** Sets this vector to the cross product between it and the other vector.
         * @param vector The other vector
         * @return This vector for chaining */
        public Vector3GDX crs(Vector3GDX vector)
        {
            return this.set(y * vector.z - z * vector.y, z * vector.x - x * vector.z, x * vector.y - y * vector.x);
        }

        /** Sets this vector to the cross product between it and the other vector.
         * @param x The x-component of the other vector
         * @param y The y-component of the other vector
         * @param z The z-component of the other vector
         * @return This vector for chaining */
        public Vector3GDX crs(float x, float y, float z)
        {
            return this.set(this.y * z - this.z * y, this.z * x - this.x * z, this.x * y - this.y * x);
        }

        /** Left-multiplies the vector by the given 4x3 column major matrix. The matrix should be composed by a 3x3 matrix representing
         * rotation and scale plus a 1x3 matrix representing the translation.
         * @param matrix The matrix
         * @return This vector for chaining */
        public Vector3GDX mul4x3(float[] matrix)
        {
            return set(x * matrix[0] + y * matrix[3] + z * matrix[6] + matrix[9], x * matrix[1] + y * matrix[4] + z * matrix[7]
                + matrix[10], x * matrix[2] + y * matrix[5] + z * matrix[8] + matrix[11]);
        }


        /** Multiplies the vector by the given {@link Quaternion}.
         * @return This vector for chaining */
        public Vector3GDX mul(QuaternionGDX quat)
        {
            return quat.transform(this);
        }

        public bool isUnit()
        {
            return isUnit(0.000000001f);
        }


        public bool isUnit(float margin)
        {
            return Math.Abs(len2() - 1f) < margin;
        }


        public bool isZero()
        {
            return x == 0 && y == 0 && z == 0;
        }


        public bool isZero(float margin)
        {
            return len2() < margin;
        }


        public bool isOnLine(Vector3GDX other, float epsilon)
        {
            return len2(y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x) <= epsilon;
        }


        public bool isOnLine(Vector3GDX other)
        {
            return len2(y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x) <= 0.000000001f;
        }


        public bool isCollinear(Vector3GDX other, float epsilon)
        {
            return isOnLine(other, epsilon) && hasSameDirection(other);
        }


        public bool isCollinear(Vector3GDX other)
        {
            return isOnLine(other) && hasSameDirection(other);
        }


        public bool isCollinearOpposite(Vector3GDX other, float epsilon)
        {
            return isOnLine(other, epsilon) && hasOppositeDirection(other);
        }


        public bool isCollinearOpposite(Vector3GDX other)
        {
            return isOnLine(other) && hasOppositeDirection(other);
        }


        public bool isPerpendicular(Vector3GDX vector)
        {
            return QuaternionGDX.IsEqual(dot(vector), 0f);
        }


        public bool isPerpendicular(Vector3GDX vector, float epsilon)
        {
            return Math.Abs(dot(vector)) <= epsilon;
        }


        public bool hasSameDirection(Vector3GDX vector)
        {
            return dot(vector) > 0;
        }


        public bool hasOppositeDirection(Vector3GDX vector)
        {
            return dot(vector) < 0;
        }


        public Vector3GDX lerp(Vector3GDX target, float alpha)
        {
            x += alpha * (target.x - x);
            y += alpha * (target.y - y);
            z += alpha * (target.z - z);
            return this;
        }


        /** Spherically interpolates between this vector and the target vector by alpha which is in the range [0,1]. The result is
         * stored in this vector.
         *
         * @param target The target vector
         * @param alpha The interpolation coefficient
         * @return This vector for chaining. */
        public Vector3GDX slerp(Vector3GDX target, float alpha)
        {
            float d = dot(target);
            // If the inputs are too close for comfort, simply linearly interpolate.
            if(d > 0.9995 || d < -0.9995) return lerp(target, alpha);

            // theta0 = angle between input vectors
            float theta0 = (float)Math.Acos(d);
            // theta = angle between this vector and result
            float theta = theta0 * alpha;

            float st = (float)Math.Sin(theta);
            float tx = target.x - x * d;
            float ty = target.y - y * d;
            float tz = target.z - z * d;
            float l2 = tx * tx + ty * ty + tz * tz;
            float dl = st * ((l2 < 0.0001f) ? 1f : 1f / (float)Math.Sqrt(l2));

            return scl((float)Math.Cos(theta)).add(tx * dl, ty * dl, tz * dl).nor();
        }


        public String toString()
        {
            return "[" + x + ", " + y + ", " + z + "]";
        }



        public Vector3GDX limit(float limit)
        {
            return limit2(limit * limit);
        }


        public Vector3GDX limit2(float limit2)
        {
            float l = len2();
            if(l > limit2)
            {
                scl((float)Math.Sqrt(limit2 / l));
            }
            return this;
        }


        public Vector3GDX setLength(float len)
        {
            return setLength2(len * len);
        }


        public Vector3GDX setLength2(float len)
        {
            float oldLen2 = len2();
            return (oldLen2 == 0 || oldLen2 == len)
                    ? this
                    : scl((float)Math.Sqrt(len / oldLen2));
        }


        public Vector3GDX clamp(float min, float max)
        {
            float l = len2();
            if(l == 0f)
                return this;
            float max2 = max * max;
            if(l > max2)
                return scl((float)Math.Sqrt(max2 / l));
            float min2 = min * min;
            if(l < min2)
                return scl((float)Math.Sqrt(min2 / l));
            return this;
        }


        public bool epsilonEquals(Vector3GDX other, float epsilon)
        {
            if(other == null) return false;
            if(Math.Abs(other.x - x) > epsilon) return false;
            if(Math.Abs(other.y - y) > epsilon) return false;
            if(Math.Abs(other.z - z) > epsilon) return false;
            return true;
        }

        /** Compares this vector with the other vector, using the supplied epsilon for fuzzy equality testing.
         * @return whether the vectors are the same. */
        public bool epsilonEquals(float x, float y, float z, float epsilon)
        {
            if(Math.Abs(x - this.x) > epsilon) return false;
            if(Math.Abs(y - this.y) > epsilon) return false;
            if(Math.Abs(z - this.z) > epsilon) return false;
            return true;
        }


        public Vector3GDX setZero()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            return this;
        }
        public Vector3GDX fromAngles(float yaw, float pitch, float roll)
        {
            return fromAnglesRad(yaw * DegreesToRadians, pitch * DegreesToRadians, roll * DegreesToRadians);
        }


        public Vector3GDX fromAnglesRad(float yaw, float pitch, float roll)
        {
            y = (float)(Math.Cos(yaw) * Math.Cos(roll));
            x = (float)(Math.Cos(pitch) * Math.Sin(yaw));
            z = (float)(Math.Sin(pitch) * Math.Sin(roll));
            return this.nor();

        }
    }
    /** A simple quaternion class.
     * @see <a href="http://en.wikipedia.org/wiki/Quaternion">http://en.wikipedia.org/wiki/Quaternion</a>
     * @author badlogicgames@gmail.com
     * @author vesuvio
     * @author xoppa */
    public class QuaternionGDX
    {
        public const float DegreesToRadians = (float)(Math.PI / 180), RadiansToDegrees = (float)(180 / Math.PI);
        private static QuaternionGDX tmp1 = new QuaternionGDX(0, 0, 0, 0);
        private static QuaternionGDX tmp2 = new QuaternionGDX(0, 0, 0, 0);

        public float x;
        public float y;
        public float z;
        public float w;

        /** Constructor, sets the four components of the quaternion.
         * @param x The x-component
         * @param y The y-component
         * @param z The z-component
         * @param w The w-component */
        public QuaternionGDX(float x, float y, float z, float w)
        {
            this.set(x, y, z, w);
        }

        public QuaternionGDX()
        {
            idt();
        }

        /** Constructor, sets the quaternion components from the given quaternion.
         * 
         * @param quaternion The quaternion to copy. */
        public QuaternionGDX(QuaternionGDX quaternion)
        {
            this.set(quaternion);
        }

        /** Constructor, sets the quaternion from the given axis vector and the angle around that axis in degrees.
         * 
         * @param axis The axis
         * @param angle The angle in degrees. */
        public QuaternionGDX(Vector3GDX axis, float angle)
        {
            this.set(axis, angle);
        }

        /** Sets the components of the quaternion
         * @param x The x-component
         * @param y The y-component
         * @param z The z-component
         * @param w The w-component
         * @return This quaternion for chaining */
        public QuaternionGDX set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            return this;
        }

        /** Sets the quaternion components from the given quaternion.
         * @param quaternion The quaternion.
         * @return This quaternion for chaining. */
        public QuaternionGDX set(QuaternionGDX quaternion)
        {
            return this.set(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        /** Sets the quaternion components from the given axis and angle around that axis.
         * 
         * @param axis The axis
         * @param angle The angle in degrees
         * @return This quaternion for chaining. */
        public QuaternionGDX set(Vector3GDX axis, float angle)
        {
            return setFromAxis(axis.x, axis.y, axis.z, angle);
        }

        /** @return a copy of this quaternion */
        public QuaternionGDX cpy()
        {
            return new QuaternionGDX(this);
        }

        /** @return the euclidean length of the specified quaternion */
        public static float len(float x, float y, float z, float w)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        /** @return the euclidean length of this quaternion */
        public float len()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
        }


        public String toString()
        {
            return "[" + x + "|" + y + "|" + z + "|" + w + "]";
        }

        /** Sets the quaternion to the given euler angles in degrees.
         * @param yaw the rotation around the y axis in degrees
         * @param pitch the rotation around the x axis in degrees
         * @param roll the rotation around the z axis degrees
         * @return this quaternion */
        public QuaternionGDX setEulerAngles(float yaw, float pitch, float roll)
        {
            return setEulerAnglesRad(yaw * DegreesToRadians, pitch * DegreesToRadians, roll
                * DegreesToRadians);
        }

        /** Sets the quaternion to the given euler angles in radians.
         * @param yaw the rotation around the y axis in radians
         * @param pitch the rotation around the x axis in radians
         * @param roll the rotation around the z axis in radians
         * @return this quaternion */
        public QuaternionGDX setEulerAnglesRad(float yaw, float pitch, float roll)
        {
            float hr = roll * 0.5f;
            float shr = (float)Math.Sin(hr);
            float chr = (float)Math.Cos(hr);
            float hp = pitch * 0.5f;
            float shp = (float)Math.Sin(hp);
            float chp = (float)Math.Cos(hp);
            float hy = yaw * 0.5f;
            float shy = (float)Math.Sin(hy);
            float chy = (float)Math.Cos(hy);
            float chy_shp = chy * shp;
            float shy_chp = shy * chp;
            float chy_chp = chy * chp;
            float shy_shp = shy * shp;

            x = (chy_shp * chr) + (shy_chp * shr); // cos(yaw/2) * sin(pitch/2) * cos(roll/2) + sin(yaw/2) * cos(pitch/2) * sin(roll/2)
            y = (shy_chp * chr) - (chy_shp * shr); // sin(yaw/2) * cos(pitch/2) * cos(roll/2) - cos(yaw/2) * sin(pitch/2) * sin(roll/2)
            z = (chy_chp * shr) - (shy_shp * chr); // cos(yaw/2) * cos(pitch/2) * sin(roll/2) - sin(yaw/2) * sin(pitch/2) * cos(roll/2)
            w = (chy_chp * chr) + (shy_shp * shr); // cos(yaw/2) * cos(pitch/2) * cos(roll/2) + sin(yaw/2) * sin(pitch/2) * sin(roll/2)
            return this;
        }

        /** Get the pole of the gimbal lock, if any. 
         * @return positive (+1) for north pole, negative (-1) for south pole, zero (0) when no gimbal lock */
        public int getGimbalPole()
        {
            float t = y * x + z * w;
            return t > 0.499f ? 1 : (t < -0.499f ? -1 : 0);
        }

        /** Get the roll euler angle in radians, which is the rotation around the z axis. Requires that this quaternion is normalized. 
         * @return the rotation around the z axis in radians (between -PI and +PI) */
        public float getRollRad()
        {
            int pole = getGimbalPole();
            return (float)(pole == 0 ? Math.Atan2(2f * (w * z + y * x), 1f - 2f * (x * x + z * z)) : pole * 2f * Math.Atan2(y, w));
        }

        /** Get the roll euler angle in degrees, which is the rotation around the z axis. Requires that this quaternion is normalized. 
         * @return the rotation around the z axis in degrees (between -180 and +180) */
        public float getRoll()
        {
            return getRollRad() * RadiansToDegrees;
        }

        /** Get the pitch euler angle in radians, which is the rotation around the x axis. Requires that this quaternion is normalized. 
         * @return the rotation around the x axis in radians (between -(PI/2) and +(PI/2)) */
        public float getPitchRad()
        {
            int pole = getGimbalPole();
            return pole == 0 ? (float)Math.Asin(VoxelLogic.Clamp(2 * (w * x - z * y), -1, 1)) : (float)(pole * Math.PI * 0.5);
        }

        /** Get the pitch euler angle in degrees, which is the rotation around the x axis. Requires that this quaternion is normalized. 
         * @return the rotation around the x axis in degrees (between -90 and +90) */
        public float getPitch()
        {
            return getPitchRad() * RadiansToDegrees;
        }

        /** Get the yaw euler angle in radians, which is the rotation around the y axis. Requires that this quaternion is normalized. 
         * @return the rotation around the y axis in radians (between -PI and +PI) */
        public float getYawRad()
        {
            return getGimbalPole() == 0 ? (float)Math.Atan2(2f * (y * w + x * z), 1f - 2f * (y * y + x * x)) : 0f;
        }

        /** Get the yaw euler angle in degrees, which is the rotation around the y axis. Requires that this quaternion is normalized. 
         * @return the rotation around the y axis in degrees (between -180 and +180) */
        public float getYaw()
        {
            return getYawRad() * RadiansToDegrees;
        }

        public static float len2(float x, float y, float z, float w)
        {
            return x * x + y * y + z * z + w * w;
        }

        /** @return the length of this quaternion without square root */
        public float len2()
        {
            return x * x + y * y + z * z + w * w;
        }

        public static bool IsEqual(float a, float b)
        {
            return Math.Abs(1 - b) < 0.000001f;
        }

        /** Normalizes this quaternion to unit length
         * @return the quaternion for chaining */
        public QuaternionGDX nor()
        {
            float len = len2();
            if(len != 0.0f && !IsEqual(len, 1f))
            {
                len = (float)Math.Sqrt(len);
                w /= len;
                x /= len;
                y /= len;
                z /= len;
            }
            return this;
        }

        /** Conjugate the quaternion.
         * 
         * @return This quaternion for chaining */
        public QuaternionGDX conjugate()
        {
            x = -x;
            y = -y;
            z = -z;
            return this;
        }

        /** Transforms the given vector using this quaternion
         * 
         * @param v Vector to transform */
        public Vector3GDX transform(Vector3GDX v)
        {
            tmp2.set(this);
            tmp2.conjugate();
            tmp2.mulLeft(tmp1.set(v.x, v.y, v.z, 0)).mulLeft(this);

            v.x = tmp2.x;
            v.y = tmp2.y;
            v.z = tmp2.z;
            return v;
        }

        /** Multiplies this quaternion with another one in the form of this = this * other
         * 
         * @param other Quaternion to multiply with
         * @return This quaternion for chaining */
        public QuaternionGDX mul(QuaternionGDX other)
        {
            float newX = this.w * other.x + this.x * other.w + this.y * other.z - this.z * other.y;
            float newY = this.w * other.y + this.y * other.w + this.z * other.x - this.x * other.z;
            float newZ = this.w * other.z + this.z * other.w + this.x * other.y - this.y * other.x;
            float newW = this.w * other.w - this.x * other.x - this.y * other.y - this.z * other.z;
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
            return this;
        }

        /** Multiplies this quaternion with another one in the form of this = this * other
         * 
         * @param x the x component of the other quaternion to multiply with
         * @param y the y component of the other quaternion to multiply with
         * @param z the z component of the other quaternion to multiply with
         * @param w the w component of the other quaternion to multiply with
         * @return This quaternion for chaining */
        public QuaternionGDX mul(float x, float y, float z, float w)
        {
            float newX = this.w * x + this.x * w + this.y * z - this.z * y;
            float newY = this.w * y + this.y * w + this.z * x - this.x * z;
            float newZ = this.w * z + this.z * w + this.x * y - this.y * x;
            float newW = this.w * w - this.x * x - this.y * y - this.z * z;
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
            return this;
        }

        /** Multiplies this quaternion with another one in the form of this = other * this
         * 
         * @param other Quaternion to multiply with
         * @return This quaternion for chaining */
        public QuaternionGDX mulLeft(QuaternionGDX other)
        {
            float newX = other.w * this.x + other.x * this.w + other.y * this.z - other.z * y;
            float newY = other.w * this.y + other.y * this.w + other.z * this.x - other.x * z;
            float newZ = other.w * this.z + other.z * this.w + other.x * this.y - other.y * x;
            float newW = other.w * this.w - other.x * this.x - other.y * this.y - other.z * z;
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
            return this;
        }

        /** Multiplies this quaternion with another one in the form of this = other * this
         * 
         * @param x the x component of the other quaternion to multiply with
         * @param y the y component of the other quaternion to multiply with
         * @param z the z component of the other quaternion to multiply with
         * @param w the w component of the other quaternion to multiply with
         * @return This quaternion for chaining */
        public QuaternionGDX mulLeft(float x, float y, float z, float w)
        {
            float newX = w * this.x + x * this.w + y * this.z - z * y;
            float newY = w * this.y + y * this.w + z * this.x - x * z;
            float newZ = w * this.z + z * this.w + x * this.y - y * x;
            float newW = w * this.w - x * this.x - y * this.y - z * z;
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
            return this;
        }

        /** Add the x,y,z,w components of the passed in quaternion to the ones of this quaternion */
        public QuaternionGDX add(QuaternionGDX quaternion)
        {
            this.x += quaternion.x;
            this.y += quaternion.y;
            this.z += quaternion.z;
            this.w += quaternion.w;
            return this;
        }

        /** Add the x,y,z,w components of the passed in quaternion to the ones of this quaternion */
        public QuaternionGDX add(float qx, float qy, float qz, float qw)
        {
            this.x += qx;
            this.y += qy;
            this.z += qz;
            this.w += qw;
            return this;
        }


        /** Sets the quaternion to an identity Quaternion
         * @return this quaternion for chaining */
        public QuaternionGDX idt()
        {
            return this.set(0, 0, 0, 1);
        }

        /** @return If this quaternion is an identity Quaternion */
        public bool isIdentity()
        {
            return IsEqual(x, 0f) && IsEqual(y, 0f) && IsEqual(z, 0f) && IsEqual(w, 1f);
        }


        // todo : the setFromAxis(v3,float) method should replace the set(v3,float) method
        /** Sets the quaternion components from the given axis and angle around that axis.
         * 
         * @param axis The axis
         * @param degrees The angle in degrees
         * @return This quaternion for chaining. */
        public QuaternionGDX setFromAxis(Vector3GDX axis, float degrees)
        {
            return setFromAxis(axis.x, axis.y, axis.z, degrees);
        }

        /** Sets the quaternion components from the given axis and angle around that axis.
         * 
         * @param axis The axis
         * @param radians The angle in radians
         * @return This quaternion for chaining. */
        public QuaternionGDX setFromAxisRad(Vector3GDX axis, float radians)
        {
            return setFromAxisRad(axis.x, axis.y, axis.z, radians);
        }

        /** Sets the quaternion components from the given axis and angle around that axis.
         * @param x X direction of the axis
         * @param y Y direction of the axis
         * @param z Z direction of the axis
         * @param degrees The angle in degrees
         * @return This quaternion for chaining. */
        public QuaternionGDX setFromAxis(float x, float y, float z, float degrees)
        {
            return setFromAxisRad(x, y, z, degrees * DegreesToRadians);
        }

        /** Sets the quaternion components from the given axis and angle around that axis.
         * @param x X direction of the axis
         * @param y Y direction of the axis
         * @param z Z direction of the axis
         * @param radians The angle in radians
         * @return This quaternion for chaining. */
        public QuaternionGDX setFromAxisRad(float x, float y, float z, float radians)
        {
            float d = Vector3GDX.len(x, y, z);
            if(d == 0f) return idt();
            d = 1f / d;
            float l_ang = (float)(radians < 0 ? (Math.PI * 2) - (-radians % (Math.PI * 2)) : radians % (Math.PI * 2));
            float l_sin = (float)Math.Sin(l_ang / 2);
            float l_cos = (float)Math.Cos(l_ang / 2);
            return this.set(d * x * l_sin, d * y * l_sin, d * z * l_sin, l_cos).nor();
        }


        /** <p>
         * Sets the Quaternion from the given x-, y- and z-axis which have to be orthonormal.
         * </p>
         * 
         * <p>
         * Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/ which in turn took it from Graphics Gem code at
         * ftp://ftp.cis.upenn.edu/pub/graphics/shoemake/quatut.ps.Z.
         * </p>
         * 
         * @param xx x-axis x-coordinate
         * @param xy x-axis y-coordinate
         * @param xz x-axis z-coordinate
         * @param yx y-axis x-coordinate
         * @param yy y-axis y-coordinate
         * @param yz y-axis z-coordinate
         * @param zx z-axis x-coordinate
         * @param zy z-axis y-coordinate
         * @param zz z-axis z-coordinate */
        public QuaternionGDX setFromAxes(float xx, float xy, float xz, float yx, float yy, float yz, float zx, float zy, float zz)
        {
            return setFromAxes(false, xx, xy, xz, yx, yy, yz, zx, zy, zz);
        }

        /** <p>
         * Sets the Quaternion from the given x-, y- and z-axis.
         * </p>
         * 
         * <p>
         * Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/ which in turn took it from Graphics Gem code at
         * ftp://ftp.cis.upenn.edu/pub/graphics/shoemake/quatut.ps.Z.
         * </p>
         * 
         * @param normalizeAxes whether to normalize the axes (necessary when they contain scaling)
         * @param xx x-axis x-coordinate
         * @param xy x-axis y-coordinate
         * @param xz x-axis z-coordinate
         * @param yx y-axis x-coordinate
         * @param yy y-axis y-coordinate
         * @param yz y-axis z-coordinate
         * @param zx z-axis x-coordinate
         * @param zy z-axis y-coordinate
         * @param zz z-axis z-coordinate */
        public QuaternionGDX setFromAxes(bool normalizeAxes, float xx, float xy, float xz, float yx, float yy, float yz, float zx,
        float zy, float zz)
        {
            if(normalizeAxes)
            {
                float lx = 1f / Vector3GDX.len(xx, xy, xz);
                float ly = 1f / Vector3GDX.len(yx, yy, yz);
                float lz = 1f / Vector3GDX.len(zx, zy, zz);
                xx *= lx;
                xy *= lx;
                xz *= lx;
                yx *= ly;
                yy *= ly;
                yz *= ly;
                zx *= lz;
                zy *= lz;
                zz *= lz;
            }
            // the trace is the sum of the diagonal elements; see
            // http://mathworld.wolfram.com/MatrixTrace.html
            float t = xx + yy + zz;

            // we protect the division by s by ensuring that s>=1
            if(t >= 0)
            { // |w| >= .5
                float s = (float)Math.Sqrt(t + 1); // |s|>=1 ...
                w = 0.5f * s;
                s = 0.5f / s; // so this division isn't bad
                x = (zy - yz) * s;
                y = (xz - zx) * s;
                z = (yx - xy) * s;
            }
            else if((xx > yy) && (xx > zz))
            {
                float s = (float)Math.Sqrt(1.0 + xx - yy - zz); // |s|>=1
                x = s * 0.5f; // |x| >= .5
                s = 0.5f / s;
                y = (yx + xy) * s;
                z = (xz + zx) * s;
                w = (zy - yz) * s;
            }
            else if(yy > zz)
            {
                float s = (float)Math.Sqrt(1.0 + yy - xx - zz); // |s|>=1
                y = s * 0.5f; // |y| >= .5
                s = 0.5f / s;
                x = (yx + xy) * s;
                z = (zy + yz) * s;
                w = (xz - zx) * s;
            }
            else
            {
                float s = (float)Math.Sqrt(1.0 + zz - xx - yy); // |s|>=1
                z = s * 0.5f; // |z| >= .5
                s = 0.5f / s;
                x = (xz + zx) * s;
                y = (zy + yz) * s;
                w = (yx - xy) * s;
            }

            return this;
        }

        /** Set this quaternion to the rotation between two vectors.
         * @param v1 The base vector, which should be normalized.
         * @param v2 The target vector, which should be normalized.
         * @return This quaternion for chaining */
        public QuaternionGDX setFromCross(Vector3GDX v1, Vector3GDX v2)
        {
            float dot = (float)VoxelLogic.Clamp(v1.dot(v2), -1f, 1f);
            float angle = (float)Math.Acos(dot);
            return setFromAxisRad(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x, angle);
        }

        /** Set this quaternion to the rotation between two vectors.
         * @param x1 The base vectors x value, which should be normalized.
         * @param y1 The base vectors y value, which should be normalized.
         * @param z1 The base vectors z value, which should be normalized.
         * @param x2 The target vector x value, which should be normalized.
         * @param y2 The target vector y value, which should be normalized.
         * @param z2 The target vector z value, which should be normalized.
         * @return This quaternion for chaining */
        public QuaternionGDX setFromCross(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float dot = (float)VoxelLogic.Clamp(Vector3GDX.dot(x1, y1, z1, x2, y2, z2), -1f, 1f);
            float angle = (float)Math.Acos(dot);
            return setFromAxisRad(y1 * z2 - z1 * y2, z1 * x2 - x1 * z2, x1 * y2 - y1 * x2, angle);
        }

        /** Spherical linear interpolation between this quaternion and the other quaternion, based on the alpha value in the range
         * [0,1]. Taken from. Taken from Bones framework for JPCT, see http://www.aptalkarga.com/bones/
         * @param end the end quaternion
         * @param alpha alpha in the range [0,1]
         * @return this quaternion for chaining */
        public QuaternionGDX slerp(QuaternionGDX end, float alpha)
        {
            float d = this.x * end.x + this.y * end.y + this.z * end.z + this.w * end.w;
            float absDot = d < 0f ? -d : d;

            // Set the first and second scale for the interpolation
            float scale0 = 1f - alpha;
            float scale1 = alpha;

            // Check if the angle between the 2 quaternions was big enough to
            // warrant such calculations
            if((1 - absDot) > 0.1)
            {// Get the angle between the 2 quaternions,
             // and then store the sin() of that angle
                float angle = (float)Math.Acos(absDot);
                float invSinTheta = 1f / (float)Math.Sin(angle);

                // Calculate the scale for q1 and q2, according to the angle and
                // it's sine value
                scale0 = ((float)Math.Sin((1f - alpha) * angle) * invSinTheta);
                scale1 = ((float)Math.Sin((alpha * angle)) * invSinTheta);
            }

            if(d < 0f) scale1 = -scale1;

            // Calculate the x, y, z and w values for the quaternion by using a
            // special form of linear interpolation for quaternions.
            x = (scale0 * x) + (scale1 * end.x);
            y = (scale0 * y) + (scale1 * end.y);
            z = (scale0 * z) + (scale1 * end.z);
            w = (scale0 * w) + (scale1 * end.w);

            // Return the interpolated quaternion
            return this;
        }

        /**
         * Spherical linearly interpolates multiple quaternions and stores the result in this Quaternion.
         * Will not destroy the data previously inside the elements of q.
         * result = (q_1^w_1)*(q_2^w_2)* ... *(q_n^w_n) where w_i=1/n.
         * @param q List of quaternions
         * @return This quaternion for chaining */
        public QuaternionGDX slerp(QuaternionGDX[] q)
        {

            //Calculate exponents and multiply everything from left to right
            float w = 1.0f / q.Length;
            set(q[0]).exp(w);
            for(int i = 1; i < q.Length; i++)
                mul(tmp1.set(q[i]).exp(w));
            nor();
            return this;
        }

        /**
         * Spherical linearly interpolates multiple quaternions by the given weights and stores the result in this Quaternion.
         * Will not destroy the data previously inside the elements of q or w.
         * result = (q_1^w_1)*(q_2^w_2)* ... *(q_n^w_n) where the sum of w_i is 1.
         * Lists must be equal in length.
         * @param q List of quaternions
         * @param w List of weights
         * @return This quaternion for chaining */
        public QuaternionGDX slerp(QuaternionGDX[] q, float[] w)
        {

            //Calculate exponents and multiply everything from left to right
            set(q[0]).exp(w[0]);
            for(int i = 1; i < q.Length; i++)
                mul(tmp1.set(q[i]).exp(w[i]));
            nor();
            return this;
        }

        /**
         * Calculates (this quaternion)^alpha where alpha is a real number and stores the result in this quaternion.
         * See http://en.wikipedia.org/wiki/Quaternion#Exponential.2C_logarithm.2C_and_power
         * @param alpha Exponent
         * @return This quaternion for chaining */
        public QuaternionGDX exp(float alpha)
        {

            //Calculate |q|^alpha
            float norm = len();
            float normExp = (float)Math.Pow(norm, alpha);

            //Calculate theta
            float theta = (float)Math.Acos(w / norm);

            //Calculate coefficient of basis elements
            float coeff = 0;
            if(Math.Abs(theta) < 0.001) //If theta is small enough, use the limit of sin(alpha*theta) / sin(theta) instead of actual value
                coeff = normExp * alpha / norm;
            else
                coeff = (float)(normExp * Math.Sin(alpha * theta) / (norm * Math.Sin(theta)));

            //Write results
            w = (float)(normExp * Math.Cos(alpha * theta));
            x *= coeff;
            y *= coeff;
            z *= coeff;

            //Fix any possible discrepancies
            nor();

            return this;
        }


        /** Get the dot product between the two quaternions (commutative).
         * @param x1 the x component of the first quaternion
         * @param y1 the y component of the first quaternion
         * @param z1 the z component of the first quaternion
         * @param w1 the w component of the first quaternion
         * @param x2 the x component of the second quaternion
         * @param y2 the y component of the second quaternion
         * @param z2 the z component of the second quaternion
         * @param w2 the w component of the second quaternion
         * @return the dot product between the first and second quaternion. */
        public static float dot(float x1, float y1, float z1, float w1, float x2, float y2,
        float z2, float w2)
        {
            return x1 * x2 + y1 * y2 + z1 * z2 + w1 * w2;
        }

        /** Get the dot product between this and the other quaternion (commutative).
         * @param other the other quaternion.
         * @return the dot product of this and the other quaternion. */
        public float dot(QuaternionGDX other)
        {
            return this.x * other.x + this.y * other.y + this.z * other.z + this.w * other.w;
        }

        /** Get the dot product between this and the other quaternion (commutative).
         * @param x the x component of the other quaternion
         * @param y the y component of the other quaternion
         * @param z the z component of the other quaternion
         * @param w the w component of the other quaternion
         * @return the dot product of this and the other quaternion. */
        public float dot(float x, float y, float z, float w)
        {
            return this.x * x + this.y * y + this.z * z + this.w * w;
        }

        /** Multiplies the components of this quaternion with the given scalar.
         * @param scalar the scalar.
         * @return this quaternion for chaining. */
        public QuaternionGDX mul(float scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
            this.w *= scalar;
            return this;
        }

        /** Get the axis angle representation of the rotation in degrees. The supplied vector will receive the axis (x, y and z values)
         * of the rotation and the value returned is the angle in degrees around that axis. Note that this method will alter the
         * supplied vector, the existing value of the vector is ignored. </p> This will normalize this quaternion if needed. The
         * received axis is a unit vector. However, if this is an identity quaternion (no rotation), then the length of the axis may be
         * zero.
         * 
         * @param axis vector which will receive the axis
         * @return the angle in degrees
         * @see <a href="http://en.wikipedia.org/wiki/Axis%E2%80%93angle_representation">wikipedia</a>
         * @see <a href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle">calculation</a> */
        public float getAxisAngle(out Vector3GDX axis)
        {
            return getAxisAngleRad(out axis) * RadiansToDegrees;
        }

        /** Get the axis-angle representation of the rotation in radians. The supplied vector will receive the axis (x, y and z values)
         * of the rotation and the value returned is the angle in radians around that axis. Note that this method will alter the
         * supplied vector, the existing value of the vector is ignored. </p> This will normalize this quaternion if needed. The
         * received axis is a unit vector. However, if this is an identity quaternion (no rotation), then the length of the axis may be
         * zero.
         * 
         * @param axis vector which will receive the axis
         * @return the angle in radians
         * @see <a href="http://en.wikipedia.org/wiki/Axis%E2%80%93angle_representation">wikipedia</a>
         * @see <a href="http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle">calculation</a> */
        public float getAxisAngleRad(out Vector3GDX axis)
        {
            axis = new Vector3GDX();
            if(this.w > 1) this.nor(); // if w>1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
            float angle = (float)(2.0 * Math.Acos(this.w));
            double s = Math.Sqrt(1 - this.w * this.w); // assuming quaternion normalised then w is less than 1, so term always positive.
            if(s < 0.000001f)
            { // test to avoid divide by zero, s is always positive due to sqrt
              // if s close to zero then direction of axis not important
                axis.x = this.x; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.y = this.y;
                axis.z = this.z;
            }
            else
            {
                axis.x = (float)(this.x / s); // normalise axis
                axis.y = (float)(this.y / s);
                axis.z = (float)(this.z / s);
            }

            return angle;
        }
        public Vector3GDX getAxis()
        {
            Vector3GDX axis = new Vector3GDX();
            if(this.w > 1) this.nor(); // if w>1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
            double s = Math.Sqrt(1 - this.w * this.w); // assuming quaternion normalised then w is less than 1, so term always positive.
            if(s < 0.000001f)
            { // test to avoid divide by zero, s is always positive due to sqrt
              // if s close to zero then direction of axis not important
                axis.x = this.x; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.y = this.y;
                axis.z = this.z;
            }
            else
            {
                axis.x = (float)(this.x / s); // normalise axis
                axis.y = (float)(this.y / s);
                axis.z = (float)(this.z / s);
            }

            return axis;
        }

        /** Get the angle in radians of the rotation this quaternion represents. Does not normalize the quaternion. Use
         * {@link #getAxisAngleRad(Vector3)} to get both the axis and the angle of this rotation. Use
         * {@link #getAngleAroundRad(Vector3)} to get the angle around a specific axis.
         * @return the angle in radians of the rotation */
        public float getAngleRad()
        {
            return (float)(2.0 * Math.Acos((this.w > 1) ? (this.w / len()) : this.w));
        }

        /** Get the angle in degrees of the rotation this quaternion represents. Use {@link #getAxisAngle(Vector3)} to get both the axis
         * and the angle of this rotation. Use {@link #getAngleAround(Vector3)} to get the angle around a specific axis.
         * @return the angle in degrees of the rotation */
        public float getAngle()
        {
            return getAngleRad() * RadiansToDegrees;
        }

        /** Get the swing rotation and twist rotation for the specified axis. The twist rotation represents the rotation around the
         * specified axis. The swing rotation represents the rotation of the specified axis itself, which is the rotation around an
         * axis perpendicular to the specified axis.
         * </p>
         * The swing and twist rotation can be used to reconstruct the original quaternion: this = swing * twist
         * 
         * @param axisX the X component of the normalized axis for which to get the swing and twist rotation
         * @param axisY the Y component of the normalized axis for which to get the swing and twist rotation
         * @param axisZ the Z component of the normalized axis for which to get the swing and twist rotation
         * @param swing will receive the swing rotation: the rotation around an axis perpendicular to the specified axis
         * @param twist will receive the twist rotation: the rotation around the specified axis
         * @see <a href="http://www.euclideanspace.com/maths/geometry/rotations/for/decomposition">calculation</a> */
        public void getSwingTwist(float axisX, float axisY, float axisZ, QuaternionGDX swing,
        QuaternionGDX twist)
        {
            float d = Vector3GDX.dot(this.x, this.y, this.z, axisX, axisY, axisZ);
            twist.set(axisX * d, axisY * d, axisZ * d, this.w).nor();
            swing.set(twist).conjugate().mulLeft(this);
        }

        /** Get the swing rotation and twist rotation for the specified axis. The twist rotation represents the rotation around the
         * specified axis. The swing rotation represents the rotation of the specified axis itself, which is the rotation around an
         * axis perpendicular to the specified axis.
         * </p>
         * The swing and twist rotation can be used to reconstruct the original quaternion: this = swing * twist
         * 
         * @param axis the normalized axis for which to get the swing and twist rotation
         * @param swing will receive the swing rotation: the rotation around an axis perpendicular to the specified axis
         * @param twist will receive the twist rotation: the rotation around the specified axis
         * @see <a href="http://www.euclideanspace.com/maths/geometry/rotations/for/decomposition">calculation</a> */
        public void getSwingTwist(Vector3GDX axis, QuaternionGDX swing, QuaternionGDX twist)
        {
            getSwingTwist(axis.x, axis.y, axis.z, swing, twist);
        }

        /** Get the angle in radians of the rotation around the specified axis. The axis must be normalized.
         * @param axisX the x component of the normalized axis for which to get the angle
         * @param axisY the y component of the normalized axis for which to get the angle
         * @param axisZ the z component of the normalized axis for which to get the angle
         * @return the angle in radians of the rotation around the specified axis */
        public float getAngleAroundRad(float axisX, float axisY, float axisZ)
        {
            float d = Vector3GDX.dot(this.x, this.y, this.z, axisX, axisY, axisZ);
            float l2 = QuaternionGDX.len2(axisX * d, axisY * d, axisZ * d, this.w);
            return IsEqual(l2, 0f) ? 0f : (float)(2.0 * Math.Acos(VoxelLogic.Clamp((float)(this.w / Math.Sqrt(l2)), -1f, 1f)));
        }

        /** Get the angle in radians of the rotation around the specified axis. The axis must be normalized.
         * @param axis the normalized axis for which to get the angle
         * @return the angle in radians of the rotation around the specified axis */
        public float getAngleAroundRad(Vector3GDX axis)
        {
            return getAngleAroundRad(axis.x, axis.y, axis.z);
        }

        /** Get the angle in degrees of the rotation around the specified axis. The axis must be normalized.
         * @param axisX the x component of the normalized axis for which to get the angle
         * @param axisY the y component of the normalized axis for which to get the angle
         * @param axisZ the z component of the normalized axis for which to get the angle
         * @return the angle in degrees of the rotation around the specified axis */
        public float getAngleAround(float axisX, float axisY, float axisZ)
        {
            return getAngleAroundRad(axisX, axisY, axisZ) * RadiansToDegrees;
        }

        /** Get the angle in degrees of the rotation around the specified axis. The axis must be normalized.
         * @param axis the normalized axis for which to get the angle
         * @return the angle in degrees of the rotation around the specified axis */
        public float getAngleAround(Vector3GDX axis)
        {
            return getAngleAround(axis.x, axis.y, axis.z);
        }
    }
}
