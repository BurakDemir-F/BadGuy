using System;
using strange.extensions.context.impl;

namespace StrangeIOCImp
{
    public class ProjectRoot : ContextView
    {
        private void Awake()
        {
            context = new ProjectContext(this);
            context.Start();
        }
    }
}