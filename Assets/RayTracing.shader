Shader "Custom/TestShader"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            struct Ray {
                float3 origin;
                float3 dir;
            };
            
            struct RayTracingMaterial {
                float4 color;
            };

            struct HitRecord {
                bool hit;
                float t; // better to have this in double precision, but this is not supported in metal.
                float3 p;
                float3 normal;
                RayTracingMaterial material;
            };

            struct Sphere {
                float3 position;
                float radius;
                RayTracingMaterial material;
            };

            float4 ViewParams;
            float4x4 CamLocalToWorldMatrix;
            
            StructuredBuffer<Sphere> Spheres;
            int NumSpheres;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            HitRecord hit_sphere(Ray ray, float3 center, float radius) {
                HitRecord result = (HitRecord)0;

                float3 oc = center - ray.origin;
                float a = dot(ray.dir, ray.dir); // length squared
                float h = dot(ray.dir, oc);
                float c = dot(oc, oc) - radius*radius;
                float discriminant = h*h - a*c;
                
                if(discriminant < 0) {
                    result.hit = false;
                } else {
                    result.t = (h - sqrt(discriminant)) / a;
                    result.p = ray.origin + result.t * ray.dir;
                    result.normal = normalize(result.p - center);
                }

                return result;
            }

            HitRecord world_hit(Ray ray) {
                HitRecord closestHit = (HitRecord)0;
                closestHit.t = 1.#INF;

                for (int i = 0; i < NumSpheres; i++) {
                    Sphere sphere = Spheres[i];
                    HitRecord record = hit_sphere(ray, sphere.position, sphere.radius);

                    if (record.hit && record.t < closestHit.t) {
                        closestHit = record;
                        closestHit.material = sphere.material;
                    }
                }

                return closestHit;
            }

            float4 frag(v2f i) : SV_Target {
                float3 viewPointLocal = float3(i.uv - 0.5, 1) * ViewParams;
                float3 viewPoint = mul(CamLocalToWorldMatrix, float4(viewPointLocal, 1));

                Ray ray;
                ray.origin = _WorldSpaceCameraPos;
                ray.dir = normalize(viewPoint - ray.origin);
                HitRecord record = hit_sphere(ray, 0, 1);

                return world_hit(ray).material.color;
            }
            ENDCG
        }
        
    }
}
