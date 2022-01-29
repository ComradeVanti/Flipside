using UnityEngine;

namespace Foxy.Flipside
{

    public class CourseManager : MonoBehaviour
    {

        [SerializeField] private CourseGen courseGen;
        [SerializeField] private SegmentBuilder segmentBuilder;

        private readonly Rng rng = Rng.NewFromCurrentTime();
        private CourseSegment? prevSegment;


        private void Awake() =>
            GenerateInitial();

        public void OnSegmentEndReached()
        {
            GenerateNext();
        }
        

        private void GenerateInitial()
        {
            for (var i = 0; i < 2; i++) GenerateNext();
        }

        private void GenerateNext()
        {
            var next = courseGen.GenerateNext(prevSegment, rng);
            segmentBuilder.Build(next);
            prevSegment = next;
        }

    }

}